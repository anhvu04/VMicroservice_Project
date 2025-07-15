using System.Text.Json;
using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Domain.GenericRepository;
using Basket.Repositories.Entities;
using Contracts.Common.Interfaces.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.AddCart;

public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly IStockService _stockService;
    private readonly IBasketRepository _cartRepository;

    public AddToCartCommandHandler(IBasketRepository cartRepository, IStockService stockService)
    {
        _cartRepository = cartRepository;
        _stockService = stockService;
    }

    public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // check stock quantity
        var stockResult = await _stockService.GetStockAsync(request.ProductId.ToString());
        if (request.Quantity > stockResult)
        {
            return Result.Failure("Not enough stock in inventory");
        }

        // get cart
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetDataByKeyAsync(request.UserId.ToString());

        // create cart if not exist
        if (string.IsNullOrEmpty(cart))
        {
            var newCart = new Domain.Entities.Cart
            {
                UserId = request.UserId,
                Items = new List<CartItems>
                {
                    new()
                    {
                        ProductId = request.ProductId,
                        ProductName = request.ProductName,
                        ProductPrice = request.ProductPrice,
                        Quantity = request.Quantity
                    }
                },
            };

            // save cart
            await _cartRepository.SetDataAsync(cartKey, newCart, TimeSpan.FromDays(180));
            return Result.Success();
        }

        var cartItem = JsonSerializer.Deserialize<Domain.Entities.Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure("Cart is null");
        }

        // check if product already exist
        var item = cartItem.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (item == null)
        {
            cartItem.Items.Add(new CartItems
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                ProductPrice = request.ProductPrice,
                Quantity = request.Quantity
            });
        }
        else
        {
            item.Quantity += request.Quantity;
        }

        // save cart
        await _cartRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }
}