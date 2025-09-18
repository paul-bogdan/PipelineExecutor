namespace PipelineExecutor.Models;

/// <summary>
/// Represents resilience configuration for a pipeline action, including retry logic and compensation.
/// </summary>
public class PipelineExecutorActionConfigResilientContext
{
    /// <summary>
    /// The number of times to retry the action on failure.
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Indicates if resilience (retry logic) is enabled for the action.
    /// </summary>
    public bool IsResilient { get; set; } = false;

    /// <summary>
    /// The interval in milliseconds between retries.
    /// </summary>
    public int RetryIntervalInMs { get; set; } = 0;

    /// <summary>
    /// Indicates if compensation should be performed before each retry.
    /// </summary>
    public bool CompensateBeforeRetry { get; set; }
}