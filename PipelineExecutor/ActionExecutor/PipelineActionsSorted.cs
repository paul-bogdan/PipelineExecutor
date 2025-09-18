namespace PipelineExecutor.ActionExecutor;

/// <summary>
/// Represents a sorted group of pipeline actions, with metadata for execution strategy.
/// </summary>
/// <typeparam name="T">The type of the command object for the pipeline actions.</typeparam>
public class PipelineActionsSorted<T> where T : class
{
    /// <summary>
    /// Indicates if this group contains multiple actions (for parallel execution).
    /// </summary>
    public bool IsMultiple { get; set; }

    /// <summary>
    /// The position/order of this group in the pipeline.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// If true, actions in this group can be executed asynchronously in parallel.
    /// </summary>
    public bool ExecuteAsyncInParallel { get; set; }

    /// <summary>
    /// Indicates if any action in this group has a compensation step.
    /// </summary>
    public bool HasCompensate { get; set; }

    /// <summary>
    /// The list of configured pipeline actions in this group.
    /// </summary>
    public List<PipelineActionConfigured<T>>? Actions { get; set; }
}