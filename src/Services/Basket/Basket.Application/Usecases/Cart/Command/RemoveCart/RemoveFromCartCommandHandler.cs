using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.GenericRepository;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.RemoveCart;

public class RemoveFromCartCommandHandler : ICommandHandler<RemoveFromCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartUtils _cartUtils;

    public RemoveFromCartCommandHandler(ICartRepository cartRepository, CartUtils cartUtils)
    {
        _cartRepository = cartRepository;
        _cartUtils = cartUtils;
    }

    public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cartKey = _cartUtils.GetCartKey(request.UserId);
            var cart = await _cartRepository.GetCartAsync(cartKey);
            if (!cart.IsSuccess)
            {
                return Result.Failure(cart.Error!);
            }

            var product = cart.Value!.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (product == null)
            {
                return Result.Failure("Product not found");
            }

            var remainQuantity = product.Quantity - request.Quantity;
            if (remainQuantity <= 0)
            {
                cart.Value.Items.Remove(product);

                // If cart is empty, delete cart
                if (cart.Value.Items.Count == 0)
                {
                    // Call grpc (schedule service) to delete cart notification job
                    await _cartUtils.ScheduledJobAsync(cart.Value);

                    var deleteCartResult = await _cartRepository.DeleteCartAsync(cartKey);
                    if (!deleteCartResult.IsSuccess)
                    {
                        return Result.Failure(CartErrors.ErrorRemovingCart);
                    }

                    return Result.Success();
                }
            }
            else
            {
                product.Quantity = remainQuantity;
            }

            // Call grpc (schedule service) to set cart notification job
            var jobId = await _cartUtils.ScheduledJobAsync(cart.Value);
            cart.Value.JobId = string.IsNullOrEmpty(jobId) ? null : jobId;

            var saveCartResult = await _cartRepository.SaveCartAsync(cartKey, cart.Value);
            if (!saveCartResult.IsSuccess)
            {
                return Result.Failure(CartErrors.ErrorRemovingCart);
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure(CartErrors.ErrorRemovingCart);
        }
    }
}