namespace PipelineExecutor.Models;

/// <summary>
/// Represents the base result for pipeline execution, including success status, errors, and result data.
/// </summary>
/// <typeparam name="T">The type of the result data.</typeparam>
public class BasePipelineExecutorResult<T> where T : class
{
    /// <summary>
    /// Indicates whether the pipeline execution was successful.
    /// </summary>
    public bool Success { get => Errors.Count == 0; }

    /// <summary>
    /// The list of errors encountered during pipeline execution.
    /// </summary>
    public List<BasePipelineExecutorErrorResult> Errors { get; set; } = new List<BasePipelineExecutorErrorResult>();

    /// <summary>
    /// The result data from the pipeline execution.
    /// </summary>
    public T Data { get; set; } = null!;
}