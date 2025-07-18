using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Contracts.Common.Interfaces.MediatR;

namespace Basket.Application.Usecases.Cart.Command.AddCart;

public class AddToCartCommand : ICommand
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}