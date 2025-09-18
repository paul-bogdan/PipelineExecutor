namespace PipelineExecutor;

/// <summary>
/// Represents a command object used in pipeline execution, containing both the command and response.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IPipelineExecutionCommandObject<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    /// <summary>
    /// Gets or sets the command to be processed in the pipeline.
    /// </summary>
    TCommand Command { get; set; }

    /// <summary>
    /// Gets or sets the response produced by the pipeline.
    /// </summary>
    TResponse Response { get; set; }
}