using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.GenericRepository;

namespace Ordering.Domain.UnitOfWork;

public interface IOrderingUnitOfWork : IUnitOfWork
{
    IOrderingGenericRepository<Order, Guid> Orders { get; }
}