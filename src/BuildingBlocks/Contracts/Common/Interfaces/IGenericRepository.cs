using System.Linq.Expressions;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Common.Interfaces;

public interface IGenericQueryRepository<TEntity, in TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    IQueryable<TEntity> FindAll(bool trackChanges = false, CancellationToken cancellationToken = default);

    IQueryable<TEntity> FindAll(bool trackChanges = false, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> FindByIdAsync(TKey id, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindByIdAsync(TKey id, bool trackChanges = false, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
}

public interface IGenericRepository<TEntity, in TKey, TContext> : IGenericQueryRepository<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}