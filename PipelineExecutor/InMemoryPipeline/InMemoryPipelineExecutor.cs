using PipelineExecutor.ActionExecutor;
using PipelineExecutor.ChangeDetection;
using PipelineExecutor.Models;

namespace PipelineExecutor.InMemoryPipeline;

/// <summary>
/// Executes a pipeline in-memory, handling actions, retries, and compensations.
/// </summary>
internal class InMemoryPipelineExecutor<TCommand, TResponse> : IBasePipelineExecutor<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    /// <summary>
    /// Initializes the executor with a list of pipeline actions.
    /// </summary>
    public InMemoryPipelineExecutor(List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>? actions)
    {
        Actions = actions;
    }
    
    // List of configured pipeline actions
    private List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>? Actions { get; set; }

    // Result of the pipeline execution
    public BasePipelineExecutorResult<IPipelineExecutionCommandObject<TCommand, TResponse>> ExecutorResult { get; private set; } = new()
    {
        Errors = new List<BasePipelineExecutorErrorResult>()
    };

    // Sorted actions for execution and compensation
    private List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>>? SortedActions { get; set; }
    private List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>>? SortedActionsCompensations { get; set; }
    private bool _compensate = false;

    /// <summary>
    /// Executes all pipeline actions and handles compensation if needed.
    /// </summary>
    public async Task ExecuteActionsAsync(IPipelineExecutionCommandObject<TCommand, TResponse> command, CancellationToken cancellationToken = default)
    {
        SortedActions = ActionListSorterHelper.PrepareForExecution(Actions ?? new());
        ArgumentNullException.ThrowIfNull(SortedActions);

        await ExecuteSortedActionsAsync(SortedActions, command, cancellationToken);

        if (_compensate && SortedActionsCompensations is not null)
        {
            await CompensateSortedActionsAsync(SortedActionsCompensations, command, cancellationToken);
        }

        ExecutorResult.Data = command;
    }

    /// <summary>
    /// Executes sorted action groups in order.
    /// </summary>
    private async Task ExecuteSortedActionsAsync(
        List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>> sortedActions,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken)
    {
        foreach (var sortedAction in sortedActions)
        {
            if (_compensate) break;
            await ExecuteActionGroupAsync(sortedAction, command, cancellationToken);
        }
    }

    /// <summary>
    /// Executes a group of actions, either in parallel or sequentially.
    /// </summary>
    private async Task ExecuteActionGroupAsync(
        PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>> actionGroup,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken)
    {
        if (actionGroup.Actions is null) return;

        var tasks = actionGroup.IsMultiple
            ? actionGroup.Actions.Select(a => ExecuteActionAsync(a, command, cancellationToken)).ToList()
            : actionGroup.Actions.Select(a => ExecuteActionAsync(a, command, cancellationToken));

        if (actionGroup.IsMultiple)
            await Task.WhenAll(tasks);
        else
            foreach (var task in tasks)
                await task;
    }

    /// <summary>
    /// Executes a single action with change detection, retry, and compensation logic.
    /// </summary>
    private async Task ExecuteActionAsync(
        PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>> actionToExecute,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken)
    {
        var changeDetectionMonitorService = new ChangeDetectionMonitorService<TCommand,TResponse>(command);
        
        try
        {
            await actionToExecute.Action.ExecuteAsync(changeDetectionMonitorService.ExecutionCopy, cancellationToken);
            changeDetectionMonitorService.Commit();
        }
        catch (Exception ex)
        {
            if (ShouldRetry(actionToExecute))
            {
                // Optionally compensate before retrying
                if (actionToExecute.Resilient is { CompensateBeforeRetry: true })
                {
                    await actionToExecute.Action.CompensateAsync(changeDetectionMonitorService.ExecutionCopy, cancellationToken);
                }
                actionToExecute.Resilient!.RetryCount--;
                await Task.Delay(actionToExecute.Resilient.RetryDelayInMilliseconds, cancellationToken);
                changeDetectionMonitorService = new ChangeDetectionMonitorService<TCommand, TResponse>(command); // reset
                await ExecuteActionAsync(actionToExecute, changeDetectionMonitorService.ExecutionCopy, cancellationToken);
            }
            else
            {
                _compensate = true;
                AddErrorResult(actionToExecute, command, ex);
            }
        }
    }

    /// <summary>
    /// Checks if the action should be retried.
    /// </summary>
    private static bool ShouldRetry(PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>> action)  
        => action.Resilient is { IsResilient: true, RetryCount: > 0 };

    /// <summary>
    /// Adds an error result to the executor result.
    /// </summary>
    private void AddErrorResult(
        PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>> action,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        Exception ex)
    {
        ExecutorResult.Errors.Add(new BasePipelineExecutorErrorResult
        {
            Message = ex.Message,
            Step = action.Position,
            ActionName =action.ActionName,
            Parameter = command,
            Compensated = false
        });
    }

    /// <summary>
    /// Compensates all sorted actions in reverse order.
    /// </summary>
    private async Task CompensateSortedActionsAsync(
        List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>> sortedActions,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken)
    {
        foreach (var sortedCompensation in sortedActions.AsEnumerable().Reverse())
        {
            await CompensateActionGroupAsync(sortedCompensation, command, cancellationToken);
        }
    }

    /// <summary>
    /// Compensates a group of actions, either in parallel or sequentially.
    /// </summary>
    private async Task CompensateActionGroupAsync(
        PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>> actionGroup,
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken)
    {
        if (actionGroup.Actions is null) return;

        var compensationTasks = actionGroup.Actions
            .Where(a => a.HasCompensate)
            .Select(a => a.Action.CompensateAsync(command, cancellationToken));

        if (actionGroup.IsMultiple)
            await Task.WhenAll(compensationTasks);
        else
            foreach (var task in compensationTasks)
                await task;
    }
}
