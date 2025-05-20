using System.Text.Json.Serialization;

namespace Basket.Repositories.Entities;

public class CartItems
{
    [JsonPropertyName("productId")] public Guid ProductId { get; set; }
    [JsonPropertyName("productName")] public string ProductName { get; set; } = null!;
    [JsonPropertyName("productPrice")] public int ProductPrice { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
}