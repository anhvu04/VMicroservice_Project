using Contracts.Common.Interfaces;
using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Implementation;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modified = _context.ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Modified or EntityState.Added or EntityState.Deleted);
        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IDateTracking addedDateEntity)
                    {
                        addedDateEntity.CreatedDate = DateTime.UtcNow;
                    }

                    if (entry.Entity is IUserTracking addedUserEntity)
                    {
                        addedUserEntity.CreatedBy = Guid.NewGuid(); // get user from context (to do)
                    }

                    break;
                case EntityState.Modified:
                    if (entry.Entity is IDateTracking modifiedDateTracking)
                    {
                        modifiedDateTracking.UpdatedDate = DateTime.UtcNow;
                        entry.Property(nameof(EntityAuditBase<Guid>.CreatedDate)).IsModified = false;
                    }

                    if (entry.Entity is IUserTracking modifiedUserEntity)
                    {
                        modifiedUserEntity.UpdatedBy = Guid.NewGuid(); // get user from context (to do)
                        entry.Property(nameof(EntityAuditBase<Guid>.CreatedBy)).IsModified = false;
                    }

                    break;
                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete deletedDateTracking)
                    {
                        deletedDateTracking.DeletedDate = DateTime.UtcNow;
                        deletedDateTracking.IsDeleted = true;
                    }

                    break;
            }
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}