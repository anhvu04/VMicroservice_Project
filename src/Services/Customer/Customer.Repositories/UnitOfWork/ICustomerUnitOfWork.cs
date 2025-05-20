using Contracts.Common.Interfaces;
using Customer.Repositories.Entities;
using Customer.Repositories.GenericRepository;
using Customer.Repositories.Persistence;

namespace Customer.Repositories.UnitOfWork;

public interface ICustomerUnitOfWork : IUnitOfWork<CustomerContext>
{
    ICustomerGenericRepository<CustomerSegment, Guid> CustomerSegment { get; }
}