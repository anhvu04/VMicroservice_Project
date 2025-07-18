using System.Text.Json.Serialization;

namespace Basket.Domain.Entities;

public class Cart
{
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
    [JsonPropertyName("items")] public List<CartItems> Items { get; set; } = [];
    [JsonPropertyName("lastModified")] public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("jobId")] public string? JobId { get; set; }
    
    
}