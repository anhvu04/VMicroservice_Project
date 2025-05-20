using System.Text.Json.Serialization;

namespace Basket.Repositories.Entities;

public class Cart
{
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
    [JsonPropertyName("items")] public List<CartItems> Items { get; set; } = [];
    [JsonPropertyName("totalPrice")] public int TotalPrice => Items.Sum(x => x.ProductPrice * x.Quantity);
}