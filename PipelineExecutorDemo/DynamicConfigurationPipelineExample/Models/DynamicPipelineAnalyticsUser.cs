namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineAnalyticsUser
{
    public Guid AnalyticsId { get; set; }
    public Guid UserId { get; set; }
    public string? Event { get; set; }
    public DateTime Timestamp { get; set; }
}