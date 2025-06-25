using Contracts.Common.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.GenericRepository;

namespace Identity.Domain.UnitOfWork;

public interface IIdentityUnitOfWork : IUnitOfWork
{
    IIdentityGenericRepository<User, Guid> Users { get; }
}