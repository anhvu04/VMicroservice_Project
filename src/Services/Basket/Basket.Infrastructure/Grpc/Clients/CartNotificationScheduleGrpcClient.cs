using System.Globalization;
using Basket.Application.Abstractions;
using ScheduledJob.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.Utils.Errors;
namespace Basket.Infrastructure.Grpc.Clients;

public class CartNotificationScheduleGrpcClient : ICartNotificationScheduleService
{
    private readonly CartNotificationScheduleService.CartNotificationScheduleServiceClient
        _cartNotificationScheduleServiceClient;

    public CartNotificationScheduleGrpcClient(
        CartNotificationScheduleService.CartNotificationScheduleServiceClient cartNotificationScheduleServiceClient)
    {
        _cartNotificationScheduleServiceClient = cartNotificationScheduleServiceClient;
    }


    public async Task<SendCartNotificationScheduleGrpcBaseResponse> SendCartNotificationScheduleAsync(
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
            throw new Exception(GrpcCalledError.BasketClientError.CartNotificationScheduleError, e);
        }
    }
}