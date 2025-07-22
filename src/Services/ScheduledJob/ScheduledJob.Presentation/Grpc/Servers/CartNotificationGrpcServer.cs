using Grpc.Core;
using MediatR;
using ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;
using ScheduledJob.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.CartNotification;

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
        var command = new SendCartNotificationScheduleGrpcBaseRequest
        {
            UserId = Guid.Parse(request.UserId),
            Items = request.Items.Select(x => new SendCartItemsNotificationScheduleGrpcBaseRequest
            {
                ProductId = Guid.Parse(x.ProductId),
                Quantity = x.Quantity,
            }).ToList(),
            LastModifiedDate = DateTime.Parse(request.LastModifiedDate),
            JobId = request.JobId
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