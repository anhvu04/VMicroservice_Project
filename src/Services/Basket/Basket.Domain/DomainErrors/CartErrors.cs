using Basket.Domain.Entities;
using Shared.Utils.Errors;

namespace Basket.Domain.DomainErrors;

public static class CartErrors
{
    public static readonly string CartNotFound = CommonErrors.NotFound<Cart>();
    public static readonly string ErrorGettingCart = "Error during accessing cart. Please try again later.";
    public static readonly string ErrorUpdatingCart = "Error during updating cart. Please try again later.";
    public static readonly string ErrorRemovingCart = "Error during removing cart. Please try again later.";
    public static readonly string CartIncludeInvalidItem = "Cart includes items that are not available. Please remove them.";
}