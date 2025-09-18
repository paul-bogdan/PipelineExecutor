namespace PipelineExecutor.Models;

/// <summary>
/// Represents the base error result for pipeline execution, including error details and compensation status.
/// </summary>
public class BasePipelineExecutorErrorResult
{
    /// <summary>
    /// The error message describing the failure.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The step number in the pipeline where the error occurred.
    /// </summary>
    public int Step { get; set; }

    /// <summary>
    /// The name of the action where the error occurred.
    /// </summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// The parameter or context associated with the error.
    /// </summary>
    public object? Parameter { get; set; }

    /// <summary>
    /// Indicates whether compensation was performed for this error.
    /// </summary>
    public bool Compensated { get; set; }
}