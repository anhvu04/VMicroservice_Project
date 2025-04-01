using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Product.Repositories.Persistence;

namespace Product.Repositories.GenericRepository;

public interface IProductGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey, ProductContext>
    where TEntity : EntityBase<TKey>
{
}