using Product.Services.Models.Requests.CatalogProduct;
using Product.Services.Models.Responses.CatalogProduct;
using Shared.Utils;
using Shared.Utils.Pagination;

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