namespace PipelineExecutor.Models;

/// <summary>
/// Represents the configuration context for a pipeline executor action,
/// including execution order, name, compensation, parallel execution, and resilience settings.
/// </summary>
public class PipelineExecutorActionConfigContext
{
    /// <summary>
    /// The position/order of the action in the pipeline.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// The unique name of the action.
    /// </summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// Indicates if the action has a compensation step.
    /// </summary>
    public bool HasCompensate { get; set; }

    /// <summary>
    /// Indicates if the action should be executed in parallel.
    /// </summary>
    public bool ExecuteInParallel { get; set; }

    /// <summary>
    /// Resilience configuration for the action.
    /// </summary>
    public PipelineExecutorActionConfigResilientContext ResilientConfig { get; set; } = new PipelineExecutorActionConfigResilientContext();
}