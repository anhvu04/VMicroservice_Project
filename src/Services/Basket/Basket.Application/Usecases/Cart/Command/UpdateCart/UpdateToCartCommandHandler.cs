using System.Text.Json;
using Basket.Application.Common;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.UpdateCart;

public class UpdateToCartCommandHandler : ICommandHandler<UpdateToCartCommand>
{
    private readonly IBasketRepository _basketRepository;

    public UpdateToCartCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Result> Handle(UpdateToCartCommand request, CancellationToken cancellationToken)
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

        product.Quantity = request.Quantity;
        await _basketRepository.SetDataAsync(cartKey, cartItem, TimeSpan.FromDays(180));
        return Result.Success();
    }
}