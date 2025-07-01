using Identity.Domain.Entities;
using Identity.Domain.GenericRepository;
using Identity.Domain.UnitOfWork;
using Identity.Persistence.GenericRepository;
using Identity.Persistence.Persistence;
using Infrastructure.Common.Implementation;

namespace Identity.Persistence.UnitOfWork;

public class IdentityUnitOfWork : UnitOfWork<IdentityContext>, IIdentityUnitOfWork
{
    private IdentityContext _context;
    private IIdentityGenericRepository<User, Guid>? _userRepository;

    public IdentityUnitOfWork(IdentityContext context) : base(context)
    {
        _context = context;
    }

    public IIdentityGenericRepository<User, Guid> Users =>
        _userRepository ??= new IdentityGenericRepository<User, Guid>(_context);
}