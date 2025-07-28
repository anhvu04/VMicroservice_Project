namespace Shared.InfrastructureGrpcModels.CartNotification;

public class SendCartNotificationScheduleGrpcBaseResponse(string jobId)
{
    public string JobId { get; set; } = jobId;
}