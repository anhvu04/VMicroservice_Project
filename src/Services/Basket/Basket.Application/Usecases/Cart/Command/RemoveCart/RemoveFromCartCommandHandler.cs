using System.Text.Json;
using Basket.Application.Common;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.RemoveCart;

public class RemoveFromCartCommandHandler : ICommandHandler<RemoveFromCartCommand>
{
    private readonly IBasketRepository _basketRepository;

    public RemoveFromCartCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var cartKey = Utils.GetCartKey(request.UserId);
        var cart = await _basketRepository.GetDataByKeyAsync(request.UserId.ToString());
        if (string.IsNullOrEmpty(cart))
        {
            return Result.Failure("Cart is empty");
        }

        var cartItem = JsonSerializer.Deserialize<Domain.Entities.Cart>(cart);
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
        await _basketRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }
}