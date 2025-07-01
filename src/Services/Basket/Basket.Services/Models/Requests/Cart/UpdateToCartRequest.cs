using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Basket.Services.Models.Requests.Cart;

public class UpdateToCartRequest
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}