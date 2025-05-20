using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
}