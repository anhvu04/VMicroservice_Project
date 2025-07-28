using System.Text.Json;
using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.Entities;
using Basket.Domain.GenericRepository;
using Microsoft.Extensions.Logging;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.AddCart;

public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatalogProductService _catalogProductService;
    private readonly CartUtils _cartUtils;
    private readonly ILogger<AddToCartCommandHandler> _logger;

    public AddToCartCommandHandler(ICartRepository cartRepository, ICatalogProductService catalogProductService,
        CartUtils cartUtils, ILogger<AddToCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _catalogProductService = catalogProductService;
        _cartUtils = cartUtils;
        _logger = logger;
    }

    public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // check product exist
        var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
            new GetListCatalogProductsByIdGrpcBaseRequest
            {
                Ids = { request.ProductId }
            });

        if (!products.IsSuccess)
        {
            return Result.Failure(CartErrors.ErrorGettingCart);
        }

        if (products.Value!.Count == 0)
        {
            return Result.Failure("Product not found");
        }

        // get cart
        var cartKey = _cartUtils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetCartAsync(cartKey);

        // create cart if not exist
        if (!cart.IsSuccess)
        {
            cart.Value = new Domain.Entities.Cart
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

            // call grpc (scheduled service) to schedule job + get jobId
            var jobIdEmptyCart = await _cartUtils.ScheduledJobAsync(cart.Value);
            cart.Value.JobId = string.IsNullOrEmpty(jobIdEmptyCart) ? null : jobIdEmptyCart;

            // save cart
            var saveCartResult = await _cartRepository.SaveCartAsync(cartKey, cart.Value);
            if (!saveCartResult.IsSuccess)
            {
                return Result.Failure(CartErrors.ErrorUpdatingCart);
            }

            return Result.Success();
        }

        // check if product already exist
        var item = cart.Value!.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (item == null)
        {
            cart.Value.Items.Add(new CartItems
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }
        else
        {
            item.Quantity += request.Quantity;
        }

        // call grpc (scheduled service) to schedule job + get jobId
        var jobId = await _cartUtils.ScheduledJobAsync(cart.Value);
        cart.Value.JobId = string.IsNullOrEmpty(jobId) ? null : jobId;

        // save cart
        var res = await _cartRepository.SaveCartAsync(cartKey, cart.Value);
        if (!res.IsSuccess)
        {
            return Result.Failure(CartErrors.ErrorUpdatingCart);
        }

        return Result.Success();
    }
}