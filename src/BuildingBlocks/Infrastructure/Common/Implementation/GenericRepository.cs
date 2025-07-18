using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Contracts.Domains.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common.Implementation;

public class GenericQueryRepository<TEntity, TKey, TContext> : IGenericQueryRepository<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericQueryRepository(TContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        if (predicate != null) return trackChanges ? _dbSet.Where(predicate) : _dbSet.Where(predicate).AsNoTracking();
        return trackChanges ? _dbSet.AsQueryable() : _dbSet.AsQueryable().AsNoTracking();
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = FindAll(predicate, trackChanges, cancellationToken);
        return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public async Task<TEntity?> FindByIdAsync(TKey id, bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindAll(x => x.Id!.Equals(id), trackChanges, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity?> FindByIdAsync(TKey id, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return FindAll(x => x.Id!.Equals(id), trackChanges, cancellationToken, includeProperties)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindAll(predicate, trackChanges, cancellationToken).AnyAsync(cancellationToken);
    }

    public async Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>>? predicate, bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAll(predicate, trackChanges, cancellationToken, includeProperties).AnyAsync(cancellationToken);
    }

    public async Task<TEntity?> FindByConditionAsync(Expression<Func<TEntity, bool>>? predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        return await FindAll(predicate, trackChanges, cancellationToken).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> FindByConditionAsync(Expression<Func<TEntity, bool>>? predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAll(predicate, trackChanges, cancellationToken, includeProperties)
            .SingleOrDefaultAsync(cancellationToken);
    }
}

public class GenericRepository<TEntity, TKey, TContext> : GenericQueryRepository<TEntity, TKey, TContext>,
    IGenericRepository<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(TContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();


    public async Task EndTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync() => _context.Database.RollbackTransactionAsync();
}