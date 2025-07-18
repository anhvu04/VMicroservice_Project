namespace Basket.Application.Usecases.Cart.Common;

public class GetCartResponse
{
    public Guid UserId { get; set; }
    public List<Items> CartItems { get; set; } = [];

    public int TotalPrice => CartItems.Sum(x =>
        x.ProductSalePrice != 0 ? x.ProductSalePrice * x.Quantity : x.ProductOriginalPrice * x.Quantity);
}

public class Items
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int ProductOriginalPrice { get; set; }
    public int ProductSalePrice { get; set; }
    public int Quantity { get; set; }
    public string? Thumbnail { get; set; }
}