using EventBus.Messages.IntegrationEvent.Interface;
using MassTransit;

namespace EventBus.Messages.IntegrationEvent.Event;

public record BasketCheckoutEvent : IntegrationBaseEvent, IBasketCheckoutEvent
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int TotalPrice { get; set; }
    public int ShippingFee { get; set; }
    public int PaymentMethod { get; set; }
    public Guid UserId { get; set; }
    public List<BasketCheckoutEventItem> Items { get; set; } = [];
}