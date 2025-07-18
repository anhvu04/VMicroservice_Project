using System.Text.Json;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Query.GetCart;

public class GetCartQueryHandler : IQueryHandler<GetCartQuery, GetCartResponse>
{
    private readonly CartUtils _cartUtils;

    public GetCartQueryHandler(CartUtils cartUtils)
    {
        _cartUtils = cartUtils;
    }

    public async Task<Result<GetCartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartUtils.GetCartAsync(request.UserId);
        if (cart == null)
        {
            return Result.Success(new GetCartResponse()
            {
                UserId = request.UserId,
                CartItems = []
            });
        }

        var enrichedCart = await _cartUtils.EnrichGetCartAsync(cart);
        if (!enrichedCart.IsSuccess)
        {
            return Result.Failure<GetCartResponse>(enrichedCart.Error!);
        }

        // Other logic such as pagination should be implemented here

        return Result.Success(enrichedCart.Value!);
    }
}