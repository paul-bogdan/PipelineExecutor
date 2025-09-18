using PipelineExecutor;
using PipelineExecutor.ActionExecutor;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Actions;

public class DynamicPipelineEmailNotification : IPipelineExecutorAction<IPipelineExecutionCommandObject<DynamicPipelineCommand,DynamicPipelineResponse>>
{
    public async Task ExecuteAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        command.Response.Notifications.Add(new DynamicPipelineNotificationUser()
        {
            NotificationId = Guid.NewGuid(),
            NotificationType = DynamicPipelineNotificationType.Email,
            Message = $"Welcome {command.Command.Name}, your user id is {command.Response.UserId}"
        });
        await Task.CompletedTask;
    }

    public Task CompensateAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}