namespace PipelineExecutor.ChangeDetection;

/// <summary>
/// Service to monitor and detect changes in a pipeline command object during execution.
/// It allows tracking, comparing, and applying changes after execution.
/// </summary>
public class ChangeDetectionMonitorService<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    // Deep copy of the original object for validation and change detection
    private readonly IPipelineExecutionCommandObject<TCommand, TResponse> _validationCopy;

    /// <summary>
    /// The original pipeline object being monitored.
    /// </summary>
    public IPipelineExecutionCommandObject<TCommand, TResponse> PipelineObject { get; private set; }

    /// <summary>
    /// The working copy used for execution and modification.
    /// </summary>
    public IPipelineExecutionCommandObject<TCommand, TResponse> ExecutionCopy { get; private set; }

    /// <summary>
    /// Initializes the monitor with deep copies for change tracking.
    /// </summary>
    public ChangeDetectionMonitorService(IPipelineExecutionCommandObject<TCommand, TResponse> pipelineObject)
    {
        PipelineObject = pipelineObject;
        _validationCopy = DeepCopyHelper.DeepCopy(pipelineObject);
        ExecutionCopy = DeepCopyHelper.DeepCopy(pipelineObject);
    }

    /// <summary>
    /// After successful execution, calculates differences and applies them to the original object.
    /// </summary>
    public void Commit()
    {
        var diffs = CalculateDifferences(_validationCopy, ExecutionCopy);
        ApplyDifferences(PipelineObject, diffs);
        // Refresh the pipeline object to ensure it reflects the latest state
        PipelineObject = DeepCopyHelper.DeepCopy(PipelineObject);
    }

    /// <summary>
    /// Compares two objects and returns a dictionary of property differences.
    /// </summary>
    private Dictionary<string, object?> CalculateDifferences(
        IPipelineExecutionCommandObject<TCommand, TResponse> oldObj,
        IPipelineExecutionCommandObject<TCommand, TResponse> newObj)
    {
        var diffs = new Dictionary<string, object?>();
        var props = typeof(IPipelineExecutionCommandObject<TCommand, TResponse>).GetProperties();

        foreach (var prop in props)
        {
            var oldVal = prop.GetValue(oldObj);
            var newVal = prop.GetValue(newObj);

            if (!Equals(oldVal, newVal))
            {
                diffs[prop.Name] = newVal;
            }
        }
        return diffs;
    }

    /// <summary>
    /// Applies the detected differences to the target object.
    /// </summary>
    private void ApplyDifferences(
        IPipelineExecutionCommandObject<TCommand, TResponse> target,
        Dictionary<string, object?> diffs)
    {
        var props = typeof(IPipelineExecutionCommandObject<TCommand, TResponse>).GetProperties();
        foreach (var diff in diffs)
        {
            var prop = props.FirstOrDefault(p => p.Name == diff.Key);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(target, diff.Value);
            }
        }
    }
}
