namespace Shared.InfrastructureServiceModels.CartNotification;

public class SendCartNotificationScheduleRequest
{
    public Guid UserId { get; set; }
    public List<SendCartItemsNotificationScheduleRequest> Items { get; set; } = null!;
    public DateTime LastModifiedDate { get; set; }
}

public class SendCartItemsNotificationScheduleRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}