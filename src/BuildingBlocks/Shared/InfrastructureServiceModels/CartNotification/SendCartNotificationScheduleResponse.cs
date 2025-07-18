namespace Shared.InfrastructureServiceModels.CartNotification;

public class SendCartNotificationScheduleResponse(string jobId)
{
    public string JobId { get; set; } = jobId;
}