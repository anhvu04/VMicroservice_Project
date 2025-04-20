using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Domain.GenericRepository;

public interface IOrderingGenericRepository<TEntity, in TKey, TContext> : IGenericRepository<TEntity, TKey, TContext>
    where TContext : DbContext where TEntity : EntityBase<TKey>
{
}