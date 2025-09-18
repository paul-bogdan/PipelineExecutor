namespace PipelineExecutor.ActionExecutor;

/// <summary>
/// Represents a configured pipeline action with execution, compensation, parallelism, and resilience settings.
/// </summary>
/// <typeparam name="T">The type of the command object for the pipeline action.</typeparam>
public class PipelineActionConfigured<T> where T : class
{
    /// <summary>
    /// The pipeline action to execute.
    /// </summary>
    public IPipelineExecutorAction<T> Action { get; set; } = null!;
    
    /// <summary>
    /// Optional name for the pipeline action, used for identification or logging.
    /// </summary>
    public string? ActionName { get; set; } 

    /// <summary>
    /// The position/order of this action in the pipeline.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Indicates if this action has a compensation step defined.
    /// </summary>
    public bool HasCompensate { get; set; }

    /// <summary>
    /// If true, this action can be executed asynchronously in parallel with others.
    /// </summary>
    public bool ExecuteAsyncInParallel { get; set; }

    /// <summary>
    /// Optional resilience configuration (e.g., retry logic) for this action.
    /// </summary>
    public PipelineActionConfiguredResilient? Resilient { get; set; }
}