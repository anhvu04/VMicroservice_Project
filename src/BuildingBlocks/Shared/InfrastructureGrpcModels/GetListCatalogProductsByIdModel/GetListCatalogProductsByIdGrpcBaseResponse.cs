namespace Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

public class GetListCatalogProductsByIdGrpcBaseResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public string? Thumbnail { get; set; }
}