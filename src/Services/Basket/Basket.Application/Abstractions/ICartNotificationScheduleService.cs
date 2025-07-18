using Shared.InfrastructureServiceModels.CartNotification;

namespace Basket.Application.Abstractions;

public interface ICartNotificationScheduleService
{
    Task<SendCartNotificationScheduleResponse> SendCartNotificationScheduleAsync(SendCartNotificationScheduleRequest scheduleRequest,
        CancellationToken cancellationToken = default);
}