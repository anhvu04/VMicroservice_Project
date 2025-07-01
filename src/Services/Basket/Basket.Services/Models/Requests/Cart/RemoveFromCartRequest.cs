using System.Text.Json.Serialization;

namespace Basket.Services.Models.Requests.Cart;

public class RemoveFromCartRequest
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}