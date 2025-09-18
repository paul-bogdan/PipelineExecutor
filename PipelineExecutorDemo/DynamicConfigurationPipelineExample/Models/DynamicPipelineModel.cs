using PipelineExecutor;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineModel : IPipelineExecutionCommandObject<DynamicPipelineCommand ,DynamicPipelineResponse>
{
    public DynamicPipelineCommand Command { get; set; } = new DynamicPipelineCommand();
    public DynamicPipelineResponse Response { get; set; } = new DynamicPipelineResponse();
}