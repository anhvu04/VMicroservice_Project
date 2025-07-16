using Contracts.Domains.Entity;
using Infrastructure.Common.Implementation;
using Product.Domain.GenericRepository;
using Product.Persistence.Persistence;

namespace Product.Persistence.GenericRepository;

public class ProductGenericRepository<TEntity, TKey> : GenericRepository<TEntity, TKey, ProductContext>,
    IProductGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    public ProductGenericRepository(ProductContext context) : base(context)
    {
    }
}