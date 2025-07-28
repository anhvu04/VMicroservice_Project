using System.Globalization;
using Basket.Application.Abstractions;
using Microsoft.Extensions.Logging;
using ScheduledJob.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.Utils;
using Shared.Utils.Errors;

namespace Basket.Infrastructure.Grpc.Clients;

public class CartNotificationScheduleGrpcClient : ICartNotificationScheduleService
{
    private readonly CartNotificationScheduleService.CartNotificationScheduleServiceClient
        _cartNotificationScheduleServiceClient;

    private readonly ILogger<CartNotificationScheduleGrpcClient> _logger;

    public CartNotificationScheduleGrpcClient(
        CartNotificationScheduleService.CartNotificationScheduleServiceClient cartNotificationScheduleServiceClient,
        ILogger<CartNotificationScheduleGrpcClient> logger)
    {
        _cartNotificationScheduleServiceClient = cartNotificationScheduleServiceClient;
        _logger = logger;
    }


    public async Task<Result<SendCartNotificationScheduleGrpcBaseResponse>> SendCartNotificationScheduleAsync(
        SendCartNotificationScheduleGrpcBaseRequest scheduleGrpcBaseRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var jobId = await _cartNotificationScheduleServiceClient.SendCartNotificationScheduleAsync(
                new CartNotificationScheduleRequest
                {
                    UserId = scheduleGrpcBaseRequest.UserId.ToString(),
                    Items =
                    {
                        scheduleGrpcBaseRequest.Items.Select(x => new CartItems
                        {
                            ProductId = x.ProductId.ToString(),
                            Quantity = x.Quantity
                        })
                    },
                    LastModifiedDate = scheduleGrpcBaseRequest.LastModifiedDate.ToString(CultureInfo.InvariantCulture),
                    JobId = scheduleGrpcBaseRequest.JobId
                });

            return new SendCartNotificationScheduleGrpcBaseResponse(jobId.JobId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, GrpcCalledError.BasketClientError.CartNotificationScheduleError);
            return Result.Failure<SendCartNotificationScheduleGrpcBaseResponse>(GrpcCalledError.BasketClientError
                .CartNotificationScheduleError);
        }
    }
}