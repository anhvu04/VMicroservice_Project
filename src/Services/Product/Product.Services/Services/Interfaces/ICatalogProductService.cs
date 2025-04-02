using Product.Services.Models.Request.CatalogProduct;
using Product.Services.Models.Response.CatalogProduct;
using Product.Services.Utils;
using Product.Services.Utils.Pagination;

namespace Product.Services.Services.Interfaces;

public interface ICatalogProductService
{
    Task<Result<PaginationResult<GetCatalogProduct>>> GetCatalogProductsAsync(GetCatalogProductsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<GetCatalogProduct>> GetCatalogProductAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> CreateCatalogProductAsync(CreateCatalogProductRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateCatalogProductAsync(Guid id, UpdateCatalogProductRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteCatalogProductAsync(Guid id, CancellationToken cancellationToken = default);
}