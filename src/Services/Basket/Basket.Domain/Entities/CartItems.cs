using System.Text.Json.Serialization;

namespace Basket.Domain.Entities;

public class CartItems
{
    [JsonPropertyName("productId")] public Guid ProductId { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
}