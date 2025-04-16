namespace Basket.Services.Models.Requests.Cart;

public class RemoveFromCartRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}