using System.Text.Json.Serialization;

namespace Basket.Application.Usecases.Cart.Command;

public class RemoveFromCartRequest
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}