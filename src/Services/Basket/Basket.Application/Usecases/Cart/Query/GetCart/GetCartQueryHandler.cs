using System.Text.Json;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Common;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Query.GetCart;

public class GetCartQueryHandler : IQueryHandler<GetCartQuery, GetCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public GetCartQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Result<GetCartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _basketRepository.GetDataByKeyAsync(cartKey);
        if (string.IsNullOrEmpty(cart))
        {
            return Result.Success(new GetCartResponse
            {
                UserId = request.UserId,
                CartItems = []
            });
        }

        var cartItem = JsonSerializer.Deserialize<Domain.Entities.Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure<GetCartResponse>("Cart is null");
        }

        var response = new GetCartResponse
        {
            UserId = request.UserId,
            CartItems = cartItem.Items.Select(item => new Items
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductPrice = item.ProductPrice,
                Quantity = item.Quantity
            }).ToList()
        };

        return Result.Success(response);
    }
}