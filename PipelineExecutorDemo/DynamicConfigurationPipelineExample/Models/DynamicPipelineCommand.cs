namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineCommand
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DynamicPipelineSource Source { get; set; }
}