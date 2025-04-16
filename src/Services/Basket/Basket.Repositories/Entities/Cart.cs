namespace Basket.Repositories.Entities;

public class Cart
{
    public Guid UserId { get; set; }
    public List<CartItems> Items { get; set; } = [];
    public int TotalPrice => Items.Sum(x => x.ProductPrice * x.Quantity);
}