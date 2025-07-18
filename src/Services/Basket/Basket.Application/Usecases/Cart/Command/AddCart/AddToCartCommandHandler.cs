using System.Text.Json;
using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.Entities;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Shared.InfrastructureServiceModels.CartNotification;
using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;
using Shared.Utils;

namespace Basket.Application.Usecases.Cart.Command.AddCart;

public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly IBasketRepository _cartRepository;
    private readonly ICartNotificationScheduleService _cartNotificationScheduleService;
    private readonly ICatalogProductService _catalogProductService;

    public AddToCartCommandHandler(IBasketRepository cartRepository,
        ICartNotificationScheduleService cartNotificationScheduleService, ICatalogProductService catalogProductService)
    {
        _cartRepository = cartRepository;
        _cartNotificationScheduleService = cartNotificationScheduleService;
        _catalogProductService = catalogProductService;
    }

    public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // check product exist
            var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
                new GetListCatalogProductsByIdRequest
                {
                    Ids = { request.ProductId }
                });

            if (products.Count == 0)
            {
                return Result.Failure("Product not found");
            }

            // get cart
            var cartKey = Utils.GetCartKey(request.UserId);
            var jsonCart = await _cartRepository.GetDataByKeyAsync(cartKey);
            Domain.Entities.Cart? cart;
            // create cart if not exist
            if (string.IsNullOrEmpty(jsonCart))
            {
                cart = new Domain.Entities.Cart
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

                // save cart
                await _cartRepository.SetDataAsync(cartKey, cart, TimeSpan.FromDays(180));
                return Result.Success();
            }

            cart = JsonSerializer.Deserialize<Domain.Entities.Cart>(jsonCart);
            if (cart == null)
            {
                return Result.Failure("Failed to deserialize cart");
            }

            // check if product already exist
            var item = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (item == null)
            {
                cart.Items.Add(new CartItems
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
            cart.JobId = await ScheduledJobAsync(cart);

            // save cart
            await _cartRepository.SetDataAsync(cartKey, cart, TimeSpan.FromDays(180));
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure(CartErrors.ErrorGettingCart);
        }
    }

    private async Task<string?> ScheduledJobAsync(Domain.Entities.Cart cart)
    {
        try
        {
            var job = await _cartNotificationScheduleService.SendCartNotificationScheduleAsync(
                new SendCartNotificationScheduleRequest
                {
                    UserId = cart.UserId,
                    Items = cart.Items.Select(x => new SendCartItemsNotificationScheduleRequest
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                });
            return job.JobId;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to schedule job" + e.Message);
            return null;
        }
    }
}