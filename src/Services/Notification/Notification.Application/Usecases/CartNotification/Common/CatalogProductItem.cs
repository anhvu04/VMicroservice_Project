namespace Notification.Application.Usecases.CartNotification.Common;

public class CatalogProductItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public string? Thumbnail { get; set; }
}