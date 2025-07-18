using Shared.InfrastructureServiceModels.CartNotification;

namespace ScheduledJob.Application.Usecases.CartNotification.Common;

public class GetCartNotificationScheduleResponse(string jobId) : SendCartNotificationScheduleResponse(jobId)
{
}