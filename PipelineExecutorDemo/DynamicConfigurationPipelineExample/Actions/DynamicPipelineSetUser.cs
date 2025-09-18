using PipelineExecutor;
using PipelineExecutor.ActionExecutor;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Actions;

public class DynamicPipelineSetUser : IPipelineExecutorAction<IPipelineExecutionCommandObject<DynamicPipelineCommand,DynamicPipelineResponse>>
{
    public async Task ExecuteAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        command.Response.UserId = Guid.NewGuid();
        command.Response.Status="User Created";
        
        await Task.CompletedTask;
    }

    public Task CompensateAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}