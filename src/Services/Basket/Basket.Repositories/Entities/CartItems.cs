namespace Basket.Repositories.Entities;

public class CartItems
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int ProductPrice { get; set; }
    public int Quantity { get; set; }
}