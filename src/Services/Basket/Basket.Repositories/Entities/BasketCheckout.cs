namespace Basket.Repositories.Entities;

public class BasketCheckout
{
    public required string RecieverName { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public int TotalPrice { get; set; }
    public Guid UserId { get; set; }
}