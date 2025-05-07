using Ordering.Domain.Entities;

namespace Ordering.Application.Usecases.Order.Common;

public record OrderModel
{
    public Guid OrderId { get; set; }
    public int TotalPrice { get; set; }
    public int ShippingFee { get; set; }
    public int TotalAmount { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public OrderStatus OrderStatus { get; set; }
}