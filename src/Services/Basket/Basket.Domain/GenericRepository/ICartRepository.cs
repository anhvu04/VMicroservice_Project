using Basket.Domain.Entities;
using Shared.Utils;

namespace Basket.Domain.GenericRepository;

public interface ICartRepository
{
    Task<Result<Cart?>> GetCartAsync(string cartKey);
    Task<Result> SaveCartAsync(string cartKey, Cart cart);
    Task<Result> DeleteCartAsync(string cartKey);
}