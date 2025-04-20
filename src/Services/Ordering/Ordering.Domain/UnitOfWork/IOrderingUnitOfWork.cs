using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Domain.UnitOfWork;

public interface IOrderingUnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
}