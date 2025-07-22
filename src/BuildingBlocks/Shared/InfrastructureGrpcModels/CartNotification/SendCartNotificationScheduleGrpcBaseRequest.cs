using Shared.MediatR;

namespace Shared.InfrastructureGrpcModels.CartNotification;

public class SendCartNotificationScheduleGrpcBaseRequest : ICommand<SendCartNotificationScheduleGrpcBaseResponse>
{
    public Guid UserId { get; set; }
    public List<SendCartItemsNotificationScheduleGrpcBaseRequest> Items { get; set; } = null!;
    public DateTime LastModifiedDate { get; set; }
    public string? JobId { get; set; }
}

public class SendCartItemsNotificationScheduleGrpcBaseRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}