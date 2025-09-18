using PipelineExecutor;
using PipelineExecutor.ActionExecutor;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Actions;

public class DynamicPipelineAnalyticsUserAction : IPipelineExecutorAction<IPipelineExecutionCommandObject<DynamicPipelineCommand,DynamicPipelineResponse>>
{
    public async Task ExecuteAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        command.Response.Analytics.Add(new DynamicPipelineAnalyticsUser()
        {
            AnalyticsId = Guid.NewGuid(),
            Event = "UserCreated",
            UserId = command.Response.UserId
        });
        await Task.CompletedTask;
    }

    public Task CompensateAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}