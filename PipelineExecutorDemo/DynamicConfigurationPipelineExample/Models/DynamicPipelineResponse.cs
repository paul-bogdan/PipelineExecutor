namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineResponse
{
    public Guid UserId { get; set; }
    public string? Status { get; set; }
    public DynamicPipelineUserDetail? UserDetail { get; set; }
    public List<DynamicPipelineNotificationUser> Notifications { get; set; } =new List<DynamicPipelineNotificationUser>();
    public List<DynamicPipelineAnalyticsUser> Analytics { get; set; } = new List<DynamicPipelineAnalyticsUser>();
}