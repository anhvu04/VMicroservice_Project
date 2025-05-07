using System.Linq.Expressions;
using Contracts.Domains;

namespace Ordering.Domain.Entities;

public class Order : EntityDateBase<Guid>
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
}

public enum OrderStatus
{
    Pending = 1,
    Processing = 2,
    Shipping = 3,
    Delivered = 4,
    Cancelled = 5
}