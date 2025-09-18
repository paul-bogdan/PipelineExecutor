namespace PipelineExecutor.ActionExecutor;

/// <summary>
/// Configuration for resilience and retry behavior of a pipeline action.
/// </summary>
public class PipelineActionConfiguredResilient
{
    /// <summary>
    /// Indicates if the action should use resilience (retry/compensation) logic.
    /// </summary>
    public bool IsResilient { get; set; }

    /// <summary>
    /// Number of times to retry the action if it fails.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Delay in milliseconds between retries.
    /// </summary>
    public int RetryDelayInMilliseconds { get; set; } = 1000;

    /// <summary>
    /// If true, compensation is performed before retrying the action.
    /// </summary>
    public bool CompensateBeforeRetry { get; set; } = false;
}