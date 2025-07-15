using Contracts.Common.Interfaces;
using Contracts.Domains;
using Contracts.Domains.Entity;
using Infrastructure.Common.Implementation;
using Product.Repositories.Persistence;

namespace Product.Repositories.GenericRepository;

public class ProductGenericRepository<TEntity, TKey> :
    GenericRepository<TEntity, TKey, ProductContext>,
    IProductGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    public ProductGenericRepository(ProductContext context) : base(context)
    {
    }
}