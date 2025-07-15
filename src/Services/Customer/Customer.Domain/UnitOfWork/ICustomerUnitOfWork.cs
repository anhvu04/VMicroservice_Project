using Contracts.Common.Interfaces;
using Customer.Domain.Entities;
using Customer.Domain.GenericRepository;

namespace Customer.Domain.UnitOfWork;

public interface ICustomerUnitOfWork : IUnitOfWork
{
    ICustomerGenericRepository<CustomerSegment, Guid> CustomerSegment { get; }
}