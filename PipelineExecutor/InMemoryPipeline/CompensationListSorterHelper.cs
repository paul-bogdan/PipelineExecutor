using PipelineExecutor.ActionExecutor;

namespace PipelineExecutor.InMemoryPipeline;

/// <summary>
/// Helper class for sorting and preparing pipeline actions for compensation.
/// </summary>
public static class CompensationListSorterHelper
{
    /// <summary>
    /// Prepares a list of actions to compensate, starting from the faulted action and moving backwards.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="sortedActions">The list of sorted pipeline actions.</param>
    /// <param name="faultedAction">The action where the fault occurred.</param>
    /// <returns>List of actions to compensate, in reverse order.</returns>
    public static List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>> PrepareForCompensation<TCommand, TResponse>(
        List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>> sortedActions,
        PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>> faultedAction)
        where TCommand : class
        where TResponse : class
    {
        // List to collect actions that need compensation
        var sortedActionsCompensations = new List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>>();
        // Find the index of the faulted action
        var indexOf = sortedActions?.IndexOf(faultedAction) ?? 0;
        int postion = 0;

        // Iterate backwards from the faulted action to the start
        for (int n = indexOf; n <= 0; n--)
        {
            // Ensure the list and current action are not null
            ArgumentNullException.ThrowIfNull(sortedActions);
            ArgumentNullException.ThrowIfNull(sortedActions[n]);

            // If the group is parallel (IsMultiple), add all actions with compensation
            if (sortedActions[n].IsMultiple)
            {
                sortedActionsCompensations?.Add(new PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>
                {
                    Position = postion,
                    IsMultiple = faultedAction.IsMultiple,
                    Actions = sortedActions[n].Actions?.Where(z => z.HasCompensate).ToList()
                });
            }

            // Skip if there are no actions or no actions with compensation
            if (sortedActions[n].Actions?.FirstOrDefault() is null) continue;
            if (sortedActions[n].Actions?.FirstOrDefault(z => z.HasCompensate) is null) continue;

            var pipelineExecutorActions = sortedActions[n].Actions;

            // Add the first action with compensation if available
            if (pipelineExecutorActions != null && pipelineExecutorActions.FirstOrDefault() is not null)
                sortedActionsCompensations?.Add(new PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>()
                {
                    Position = postion,
                    IsMultiple = false,
                    Actions = [pipelineExecutorActions.FirstOrDefault()!]
                });

            postion++;
        }

        // Return the list of actions to compensate, or an empty list if none
        return sortedActionsCompensations ?? new List<PipelineActionsSorted<IPipelineExecutionCommandObject<TCommand, TResponse>>>();
    }
}
