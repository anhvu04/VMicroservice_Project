using System.Linq.Expressions;
using Contracts.Common.Events;
using Ordering.Domain.OrderAggregate.Events;

namespace Ordering.Domain.Entities;

public class Order : DateTrackingEventEntity<Guid>
{
    public Guid UserId { get; set; }
    public int TotalPrice { get; set; }
    public int ShippingFee { get; set; }
    public int TotalAmount { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = [];

    public static Expression<Func<Order, object>> GetSortValue(string propertyName)
    {
        return propertyName switch
        {
            "totalPrice" => o => o.TotalPrice,
            "totalAmount" => o => o.TotalAmount,
            _ => o => o.CreatedDate
        };
    }

    public void AddedOrder()
    {
        AddDomainEvent(new OrderCreatedEvent(Id, UserId, TotalPrice, ShippingFee, TotalAmount, FirstName + LastName,
            Email,
            Address, PhoneNumber, OrderStatus, PaymentMethod, OrderDetails.ToList()));
    }

    public void DeletedOrder()
    {
        AddDomainEvent(new OrderDeletedEvent(Id));
    }
}

public enum OrderStatus
{
    Pending = 1,
    Processing = 2,
    Shipping = 3,
    Delivered = 4,
    Cancelled = 5
}

public enum PaymentMethod
{
    Cod = 1,
    BankTransfer = 2
}