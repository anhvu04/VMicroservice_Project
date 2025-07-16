using Infrastructure.Common.Implementation;
using Product.Domain.Entities;
using Product.Domain.GenericRepository;
using Product.Domain.UnitOfWork;
using Product.Persistence.GenericRepository;
using Product.Persistence.Persistence;

namespace Product.Persistence.UnitOfWork;

public class ProductUnitOfWork : UnitOfWork<ProductContext>, IProductUnitOfWork
{
    private readonly ProductContext _context;
    private IProductGenericRepository<CatalogProduct, Guid>? _catalogProducts;

    public ProductUnitOfWork(ProductContext context) : base(context)
    {
        _context = context;
    }

    public IProductGenericRepository<CatalogProduct, Guid> CatalogProduct =>
        _catalogProducts ??= new ProductGenericRepository<CatalogProduct, Guid>(_context);
}