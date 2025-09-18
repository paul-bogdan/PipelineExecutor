using PipelineExecutor;
using PipelineExecutor.ActionExecutor;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Actions;

public class DynamicPipelineValidateCommandAction : IPipelineExecutorAction<IPipelineExecutionCommandObject<DynamicPipelineCommand,DynamicPipelineResponse>>
{
    public async Task ExecuteAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(command.Command.Name))
        {
            throw new ArgumentException("Name is required");
        }

        if (string.IsNullOrWhiteSpace(command.Command.Email))
        {
            throw new ArgumentException("Email is required");
        }

        if (!command.Command.Email.Contains("@"))
        {
            throw new ArgumentException("Email is not valid");
        }
        await Task.CompletedTask;
    }

    public Task CompensateAsync(IPipelineExecutionCommandObject<DynamicPipelineCommand, DynamicPipelineResponse> command, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}