using Grpc.Core;
using HangFire.Presentation.Grpc.Protos;
using MediatR;
using ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;
using Shared.InfrastructureServiceModels.CartNotification;

namespace ScheduledJob.Presentation.Grpc.Servers;

public class CartNotificationGrpcServer : CartNotificationScheduleService.CartNotificationScheduleServiceBase
{
    private readonly ISender _sender;

    public CartNotificationGrpcServer(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<CartNotificationScheduleResponse> SendCartNotificationSchedule(
        CartNotificationScheduleRequest request, ServerCallContext context)
    {
        var command = new SendCartNotificationScheduleCommand
        {
            UserId = Guid.Parse(request.UserId),
            Items = request.Items.Select(x => new SendCartItemsNotificationScheduleRequest
            {
                ProductId = Guid.Parse(x.ProductId),
                Quantity = x.Quantity,
            }).ToList(),
            LastModifiedDate = DateTime.Parse(request.LastModifiedDate)
        };
        var response = await _sender.Send(command, context.CancellationToken);
        if (!response.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, response.Error!));
        }

        return new CartNotificationScheduleResponse
        {
            JobId = response.Value!.JobId,
        };
    }
}