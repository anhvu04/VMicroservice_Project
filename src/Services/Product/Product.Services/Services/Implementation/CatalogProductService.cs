using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.Repositories.Entities;
using Product.Repositories.Persistence;
using Product.Repositories.UnitOfWork;
using Product.Services.Services.Interfaces;

namespace Product.Services.Services.Implementation;

public class CatalogProductService : ICatalogProductService
{
    private readonly IProductUnitOfWork _unitOfWork;

    public CatalogProductService(IProductUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CatalogProduct>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CatalogProducts.FindAll(cancellationToken: cancellationToken)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}