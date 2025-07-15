namespace Basket.Application.Usecases.Cart.Common;

public class GetCartResponse
{
    public Guid UserId { get; set; }
    public List<Items> CartItems { get; set; } = [];
    public int TotalPrice => CartItems.Sum(x => x.ProductPrice * x.Quantity);
}

public class Items
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int ProductPrice { get; set; }
    public int Quantity { get; set; }
}