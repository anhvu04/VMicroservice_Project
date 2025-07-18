using EventBus.Messages.IntegrationEvent.Interface;

namespace EventBus.Messages.IntegrationEvent.Event;

public record CartNotificationScheduleEvent : IntegrationBaseEvent, ICartNotificationScheduleEvent
{
    public Guid UserId { get; set; }
    public List<CartItemNotificationScheduleEvent> Items { get; set; } = [];
    public DateTime LastModifiedDate { get; set; }
}