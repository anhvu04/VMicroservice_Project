using System.Text.Json.Serialization;
using Contracts.Common.Interfaces.MediatR;

namespace Basket.Application.Usecases.Cart.Command.RemoveCart;

public class RemoveFromCartCommand : ICommand
{
    [JsonIgnore] public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}