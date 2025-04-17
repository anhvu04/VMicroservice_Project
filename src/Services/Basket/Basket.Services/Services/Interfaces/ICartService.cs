using Basket.Services.Models.Requests.Cart;
using Basket.Services.Models.Responses.Cart;
using Infrastructure.Utils;
using Infrastructure.Utils.Pagination;

namespace Basket.Services.Services.Interfaces;

public interface ICartService
{
    Task<Result> AddToCartAsync(AddToCartRequest request);
    Task<Result> UpdateToCartAsync(UpdateToCartRequest request);
    Task<Result> RemoveFromCartAsync(RemoveFromCartRequest request);
    Task<Result<GetCartResponse>> GetCartAsync(Guid userId, GetCartRequest request);
}