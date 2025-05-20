using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Basket.Services.Models.Requests.Checkout;

public class CheckoutRequest
{
    public required string FirstName { get; set; } = null!;
    public required string LastName { get; set; } = null!;
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public int ShippingFee { get; set; }
    public int PaymentMethod { get; set; }
    [JsonIgnore] public Guid UserId { get; set; }
}