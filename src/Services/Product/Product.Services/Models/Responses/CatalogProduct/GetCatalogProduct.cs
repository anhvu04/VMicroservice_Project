namespace Product.Services.Models.Responses.CatalogProduct;

public class GetCatalogProduct
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public string? Thumbnail { get; set; }
}