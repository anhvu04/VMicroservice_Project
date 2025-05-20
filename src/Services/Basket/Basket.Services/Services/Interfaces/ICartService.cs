using Basket.Services.Models.Requests.Cart;
using Basket.Services.Models.Responses.Cart;
using Shared.Utils;

namespace Basket.Services.Services.Interfaces;

public interface ICartService
{
    Task<Result> AddToCartAsync(AddToCartRequest request);
    Task<Result> UpdateToCartAsync(UpdateToCartRequest request);
    Task<Result> RemoveFromCartAsync(RemoveFromCartRequest request);
    Task<Result<GetCartResponse>> GetCartAsync(Guid userId);
    Task<Result> DeleteCartAsync(Guid userId);
}