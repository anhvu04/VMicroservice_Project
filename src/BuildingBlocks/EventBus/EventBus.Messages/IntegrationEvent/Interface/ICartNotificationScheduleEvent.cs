using MassTransit;

namespace EventBus.Messages.IntegrationEvent.Interface;

[ExcludeFromTopology]
public interface ICartNotificationScheduleEvent : IIntegrationBaseEvent
{
    public Guid UserId { get; set; }
    public List<CartItemNotificationScheduleEvent> Items { get; set; }
    public DateTime LastModifiedDate { get; set; }
}

public record CartItemNotificationScheduleEvent
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}