using Contracts.Domains.EventsEntity;
using Ordering.Domain.Entities;

namespace Ordering.Domain.OrderAggregate.Events;

public class OrderCreatedEvent : BaseEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public int TotalPrice { get; set; }
    public int ShippingFee { get; set; }
    public int TotalAmount { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }

    public OrderCreatedEvent(Guid orderId ,Guid userId, int totalPrice, int shippingFee, int totalAmount, string fullName,
        string email, string address, string phoneNumber, OrderStatus orderStatus, PaymentMethod paymentMethod,
        List<OrderDetail> orderDetails)
    {
        OrderId = orderId;
        UserId = userId;
        TotalPrice = totalPrice;
        ShippingFee = shippingFee;
        TotalAmount = totalAmount;
        FullName = fullName;
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
        OrderStatus = orderStatus;
        PaymentMethod = paymentMethod;
        OrderDetails = orderDetails;
    }
}