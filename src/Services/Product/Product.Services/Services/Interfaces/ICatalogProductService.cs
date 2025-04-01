using Product.Repositories.Entities;

namespace Product.Services.Services.Interfaces;

public interface ICatalogProductService
{
    Task<List<CatalogProduct>> GetProductsAsync(CancellationToken cancellationToken = default);
}