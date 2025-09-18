using PipelineExecutor.Models;

namespace PipelineExecutor;

/// <summary>
/// Defines the contract for a pipeline that processes a sequence of actions
/// with a specified command and response type.
/// </summary>
/// <typeparam name="TCommand">The type of the command object.</typeparam>
/// <typeparam name="TResponse">The type of the response object.</typeparam>
public interface IBasePipeline<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    /// <summary>
    /// Processes the pipeline actions asynchronously using the provided command object.
    /// </summary>
    /// <param name="command">The command object to process.</param>
    /// <param name="cancellationToken">A cancellation token for the operation.</param>
    Task ProcessActionsAsync(
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the result of the pipeline execution.
    /// </summary>
    BasePipelineExecutorResult<IPipelineExecutionCommandObject<TCommand, TResponse>> ExecutorResult { get; }
}