using Basket.Services.Models.Requests.Checkout;
using Shared.Utils;

namespace Basket.Services.Services.Interfaces;

public interface ICheckoutService
{
    Task<Result> CheckoutAsync(CheckoutRequest request);
}