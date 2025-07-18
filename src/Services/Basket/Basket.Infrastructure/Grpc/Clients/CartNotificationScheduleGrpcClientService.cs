using System.Globalization;
using Basket.Application.Abstractions;
using HangFire.Presentation.Grpc.Protos;
using Shared.InfrastructureServiceModels.CartNotification;
using CartItems = HangFire.Presentation.Grpc.Protos.CartItems;

namespace Basket.Infrastructure.Grpc.Clients;

public class CartNotificationScheduleGrpcClientService : ICartNotificationScheduleService
{
    private readonly CartNotificationScheduleService.CartNotificationScheduleServiceClient
        _cartNotificationScheduleServiceClient;

    public CartNotificationScheduleGrpcClientService(
        CartNotificationScheduleService.CartNotificationScheduleServiceClient cartNotificationScheduleServiceClient)
    {
        _cartNotificationScheduleServiceClient = cartNotificationScheduleServiceClient;
    }


    public async Task<SendCartNotificationScheduleResponse> SendCartNotificationScheduleAsync(
        SendCartNotificationScheduleRequest scheduleRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var jobId = await _cartNotificationScheduleServiceClient.SendCartNotificationScheduleAsync(
                new CartNotificationScheduleRequest
                {
                    UserId = scheduleRequest.UserId.ToString(),
                    Items =
                    {
                        scheduleRequest.Items.Select(x => new CartItems
                        {
                            ProductId = x.ProductId.ToString(),
                            Quantity = x.Quantity
                        })
                    },
                    LastModifiedDate = scheduleRequest.LastModifiedDate.ToString(CultureInfo.InvariantCulture)
                });

            return new SendCartNotificationScheduleResponse(jobId.JobId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}