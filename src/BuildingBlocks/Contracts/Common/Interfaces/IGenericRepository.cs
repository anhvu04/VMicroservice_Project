using System.Linq.Expressions;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Common.Interfaces;

public interface IGenericQueryRepository<TEntity, in TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
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

    Task<TEntity?> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
}

public interface IGenericRepository<TEntity, in TKey, TContext> : IGenericQueryRepository<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);
    void UpdateRange(IEnumerable<TEntity> entities);
    void DeleteRange(IEnumerable<TEntity> entities);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}