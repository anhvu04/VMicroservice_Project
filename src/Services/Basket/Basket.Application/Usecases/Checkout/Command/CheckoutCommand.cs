using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Contracts.Common.Interfaces.MediatR;

namespace Basket.Application.Usecases.Checkout.Command;

public class CheckoutCommand : ICommand
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    public int ShippingFee { get; set; }
    public int PaymentMethod { get; set; }
    [JsonIgnore] public Guid UserId { get; set; }
}