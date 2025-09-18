using PipelineExecutor.ActionExecutor;

namespace PipelineExecutor.InMemoryPipeline;

/// <summary>
/// Helper for sorting and grouping pipeline actions for execution.
/// </summary>
public static class ActionListSorterHelper
{
    /// <summary>
    /// Sorts and groups actions by position and parallel execution flag.
    /// </summary>
    /// <typeparam name="TCommand">Pipeline command type.</typeparam>
    /// <typeparam name="TResponse">Pipeline response type.</typeparam>
    /// <param name="actions">List of pipeline actions to sort and group.</param>
    /// <returns>List of grouped and sorted pipeline actions.</returns>
    public static List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand,TResponse>>> PrepareForExecution<TCommand,TResponse>(
        List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand,TResponse>>> actions)
        where TCommand : class
        where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(actions);
        var sortedActions = new List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand,TResponse>>>();

        // Sort actions by their position and group them for parallel or sequential execution
        foreach (var action in actions.OrderBy(z => z.Position))
        {
            if (action.ExecuteAsyncInParallel)
            {
                // Try to add to the last parallel group if possible
                var lastSortedAction = sortedActions.LastOrDefault();
                if (lastSortedAction?.IsMultiple == true)
                {
                    if (sortedActions[sortedActions.IndexOf(lastSortedAction)].Actions is null)
                    {
                        // If no actions exist in the group, add a new group
                        AddSortedAction(action, sortedActions);
                    }
                    else
                    {
                        // Add to the existing parallel group
                        sortedActions[sortedActions.IndexOf(lastSortedAction)].Actions?.Add(action);
                    }
                }
                else
                {
                    // Start a new parallel group
                    AddSortedAction(action, sortedActions);
                }
            }
            else
            {
                // Always start a new group for sequential actions
                AddSortedAction(action, sortedActions);
            }
        }
        return sortedActions;
    }

    /// <summary>
    /// Adds a new sorted action group to the list.
    /// </summary>
    private static void AddSortedAction<TCommand, TResponse>(
        PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>> action,
        List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>> sortedAction)
        where TCommand : class
        where TResponse : class
    {
        sortedAction?.Add(new PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>
        {
            Position = action.Position,
            Actions = new List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>
            {
                new()
                {
                    Action = action.Action,
                    ActionName = action.ActionName,
                    ExecuteAsyncInParallel = action.ExecuteAsyncInParallel,
                    HasCompensate = action.HasCompensate,
                    Position = action.Position
                }
            },
            IsMultiple = action.ExecuteAsyncInParallel
        });
    }
}
