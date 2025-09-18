using PipelineExecutor.Models;

namespace PipelineExecutor.ChannelsPipeline;
/// <summary>
/// TODO : This is a placeholder for a Channels-based pipeline executor implementation.
/// The actual implementation should utilize channels to manage and execute pipeline actions.
/// This class currently throws NotImplementedException for its methods and properties.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ChannelsPipelineExecutor<TCommand,TResponse>:IBasePipelineExecutor<TCommand,TResponse>  where TCommand : class where TResponse : class
{
    public BasePipelineExecutorResult<IPipelineExecutionCommandObject<TCommand, TResponse>> ExecutorResult { get; }
    public Task ExecuteActionsAsync(IPipelineExecutionCommandObject<TCommand, TResponse> command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}