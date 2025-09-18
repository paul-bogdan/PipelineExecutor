namespace PipelineExecutor.ActionExecutor;

/// <summary>
/// Defines a pipeline action with asynchronous execution and compensation logic.
/// </summary>
/// <typeparam name="TCommand">The type of the command object for the action.</typeparam>
public interface IPipelineExecutorAction<in TCommand> where TCommand : class 
{
    /// <summary>
    /// Executes the action asynchronously.
    /// </summary>
    /// <param name="command">The command object for the action.</param>
    /// <param name="cancellation">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(TCommand command, CancellationToken cancellation);

    /// <summary>
    /// Performs compensation asynchronously if the action needs to be reverted.
    /// </summary>
    /// <param name="command">The command object for the action.</param>
    /// <param name="cancellation">Cancellation token.</param>
    /// <returns>A task representing the asynchronous compensation operation.</returns>
    Task CompensateAsync(TCommand command, CancellationToken cancellation);
}