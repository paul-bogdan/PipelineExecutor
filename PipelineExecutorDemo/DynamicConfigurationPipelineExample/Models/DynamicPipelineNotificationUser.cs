namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

public class DynamicPipelineNotificationUser
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public string? Message { get; set; }
    public DynamicPipelineNotificationType NotificationType { get; set; }
   
}