namespace PipelineExecutor.Models;

/// <summary>
/// Represents the context for pipeline execution, including configuration options and action contexts.
/// </summary>
public interface IPipelineExecutorConfig
{
    /// <summary>
    /// Indicates whether to use channels for pipeline execution.
    /// </summary>
    bool UseChannels { get; }

    /// <summary>
    /// Indicates whether to use in-memory pipeline actions.
    /// </summary>
    bool UseInMemoryPipelineActions { get; }


    /// <summary>
    /// The list of action configuration contexts for the pipeline.
    /// </summary>
    List<PipelineExecutorActionConfigContext>? ActionConfigContexts { get; set; }
}