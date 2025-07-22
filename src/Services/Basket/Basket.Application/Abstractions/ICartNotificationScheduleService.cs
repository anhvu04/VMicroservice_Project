using Shared.InfrastructureGrpcModels.CartNotification;

namespace Basket.Application.Abstractions;

public interface ICartNotificationScheduleService
{
    Task<SendCartNotificationScheduleGrpcBaseResponse> SendCartNotificationScheduleAsync(SendCartNotificationScheduleGrpcBaseRequest scheduleGrpcBaseRequest,
        CancellationToken cancellationToken = default);
}