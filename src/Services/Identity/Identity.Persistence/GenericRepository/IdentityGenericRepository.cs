using Contracts.Domains;
using Contracts.Domains.Entity;
using Identity.Domain.GenericRepository;
using Identity.Persistence.Persistence;
using Infrastructure.Common.Implementation;

namespace Identity.Persistence.GenericRepository;

public class IdentityGenericRepository<TEntity, TKey> : GenericRepository<TEntity, TKey, IdentityContext>,
    IIdentityGenericRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    public IdentityGenericRepository(IdentityContext context) : base(context)
    {
    }
}