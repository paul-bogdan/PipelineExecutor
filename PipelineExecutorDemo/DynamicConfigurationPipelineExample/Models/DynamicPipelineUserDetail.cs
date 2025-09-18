namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineUserDetail
{
    public Guid UserId { get; set; }
    public string? NormalizedName { get; set; }
    public string? NormalizedEmail { get; set; }
}