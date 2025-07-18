using System.Text.Json.Serialization;
using Basket.Application.Usecases.Cart.Common;
using Contracts.Common.Interfaces.MediatR;

namespace Basket.Application.Usecases.Cart.Query.GetCart;

public class GetCartQuery : IQuery<GetCartResponse>
{
    public GetCartQuery(Guid userId)
    {
        UserId = userId;
    }

    [JsonIgnore] public Guid UserId { get; set; }
}