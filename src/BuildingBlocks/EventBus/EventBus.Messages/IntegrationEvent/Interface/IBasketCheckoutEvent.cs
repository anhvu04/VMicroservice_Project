using MassTransit;

namespace EventBus.Messages.IntegrationEvent.Interface;

[ExcludeFromTopology]
public interface IBasketCheckoutEvent : IIntegrationBaseEvent
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int TotalPrice { get; set; }
    public int ShippingFee { get; set; }
    public int PaymentMethod { get; set; }
    public Guid UserId { get; set; }
    public List<BasketCheckoutEventItem> Items { get; set; }
}

public record BasketCheckoutEventItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int ProductOriginalPrice { get; set; }
    public int ProductSalePrice { get; set; }
    public int Quantity { get; set; }
    public string? Thumbnail { get; set; }
}