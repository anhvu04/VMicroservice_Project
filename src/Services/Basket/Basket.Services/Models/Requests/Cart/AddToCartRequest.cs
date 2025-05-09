using System.ComponentModel.DataAnnotations;

namespace Basket.Services.Models.Requests.Cart;

public class AddToCartRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int ProductPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}