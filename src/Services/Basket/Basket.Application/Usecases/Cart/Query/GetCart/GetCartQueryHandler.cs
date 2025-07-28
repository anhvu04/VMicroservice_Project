using System.Text.Json;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.GenericRepository;
using Shared.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Query.GetCart;

public class GetCartQueryHandler : IQueryHandler<GetCartQuery, GetCartResponse>
{
    private readonly CartUtils _cartUtils;
    private readonly ICartRepository _cartRepository;

    public GetCartQueryHandler(CartUtils cartUtils, ICartRepository cartRepository)
    {
        _cartUtils = cartUtils;
        _cartRepository = cartRepository;
    }

    public async Task<Result<GetCartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        // Get cart
        var cartKey = _cartUtils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetCartAsync(cartKey);
        if (!cart.IsSuccess)
        {
            return Result.Success(new GetCartResponse
            {
                UserId = request.UserId,
                CartItems = []
            });
        }

        var enrichedCart = await _cartUtils.EnrichGetCartAsync(cart.Value!);
        if (!enrichedCart.IsSuccess)
        {
            return Result.Failure<GetCartResponse>(enrichedCart.Error!);
        }

        // Other logic such as pagination should be implemented here

        return Result.Success(enrichedCart.Value!);
    }
}