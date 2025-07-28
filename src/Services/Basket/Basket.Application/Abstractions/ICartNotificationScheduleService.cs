using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.Utils;

namespace Basket.Application.Abstractions;

public interface ICartNotificationScheduleService
{
    Task<Result<SendCartNotificationScheduleGrpcBaseResponse>> SendCartNotificationScheduleAsync(SendCartNotificationScheduleGrpcBaseRequest scheduleGrpcBaseRequest,
        CancellationToken cancellationToken = default);
}