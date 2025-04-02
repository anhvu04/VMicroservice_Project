using Infrastructure.Common.Implementation;
using Product.Repositories.Entities;
using Product.Repositories.GenericRepository;
using Product.Repositories.Persistence;

namespace Product.Repositories.UnitOfWork;

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