namespace PipelineExecutor.Models;

/// <summary>
/// Provides the context for pipeline execution, including configuration options and action contexts.
/// </summary>
public class PipelineExecutorConfig : IPipelineExecutorConfig
{
    /// <summary>
    /// Indicates whether to use channels for pipeline execution.
    /// </summary>
    public bool UseChannels
    {
        get => false;
    }

    /// <summary>
    /// Indicates whether to use in-memory pipeline actions.
    /// </summary>
    public bool UseInMemoryPipelineActions { get=> true ;}

    /// <summary>
    /// The list of action configuration contexts for the pipeline.
    /// </summary>
    public List<PipelineExecutorActionConfigContext>? ActionConfigContexts { get; set; }
}