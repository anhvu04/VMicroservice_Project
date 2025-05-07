using System.Text.Json;
using Basket.Repositories.Entities;
using Basket.Repositories.Repositories.Interfaces;
using Basket.Services.Models.Requests.Cart;
using Basket.Services.Models.Responses.Cart;
using Basket.Services.Services.Interfaces;
using Shared.Utils;

namespace Basket.Services.Services.Implementation;

public class CartService : ICartService
{
    private readonly IBasketRepository _cartRepository;

    public CartService(IBasketRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result> AddToCartAsync(AddToCartRequest request)
    {
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetDataByKeyAsync(request.UserId.ToString());
        if (string.IsNullOrEmpty(cart))
        {
            var newCart = new Cart
            {
                UserId = request.UserId,
                Items = new List<CartItems>
                {
                    new()
                    {
                        ProductId = request.ProductId,
                        Quantity = request.Quantity
                    }
                },
            };
            await _cartRepository.SetDataAsync(cartKey, newCart, TimeSpan.FromDays(180));
            return Result.Success();
        }

        var cartItem = JsonSerializer.Deserialize<Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure("Cart is null");
        }

        var item = cartItem.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (item == null)
        {
            cartItem.Items.Add(new CartItems
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }
        else
        {
            item.Quantity += request.Quantity;
        }

        await _cartRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }

    public async Task<Result> UpdateToCartAsync(UpdateToCartRequest request)
    {
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetDataByKeyAsync(request.UserId.ToString());
        if (string.IsNullOrEmpty(cart))
        {
            return Result.Failure("Cart is empty");
        }

        var cartItem = JsonSerializer.Deserialize<Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure("Cart is null");
        }

        var product = cartItem.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (product == null)
        {
            return Result.Failure("Product not found");
        }

        product.Quantity = request.Quantity;
        await _cartRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }

    public async Task<Result> RemoveFromCartAsync(RemoveFromCartRequest request)
    {
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetDataByKeyAsync(request.UserId.ToString());
        if (string.IsNullOrEmpty(cart))
        {
            return Result.Failure("Cart is empty");
        }

        var cartItem = JsonSerializer.Deserialize<Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure("Cart is null");
        }

        var product = cartItem.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (product == null)
        {
            return Result.Failure("Product not found");
        }

        cartItem.Items.Remove(product);
        await _cartRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }

    public async Task<Result<GetCartResponse>> GetCartAsync(Guid userId, GetCartRequest request)
    {
        var cartKey = Utils.GetCartKey(userId);
        var cart = await _cartRepository.GetDataByKeyAsync(cartKey);
        if (string.IsNullOrEmpty(cart))
        {
            return Result.Success(new GetCartResponse
            {
                UserId = userId,
                CartItems = []
            });
        }

        var cartItem = JsonSerializer.Deserialize<Cart>(cart);
        if (cartItem == null)
        {
            return Result.Failure<GetCartResponse>("Cart is null");
        }

        var response = new GetCartResponse
        {
            UserId = userId,
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