using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.MediatR;

namespace Basket.Application.Usecases.Cart.Command.RemoveCart;

public class RemoveFromCartCommand : ICommand
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]

    public int Quantity { get; set; }
}