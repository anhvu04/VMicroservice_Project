using Customer.Domain.Entities;
using Customer.Domain.GenericRepository;
using Customer.Domain.UnitOfWork;
using Customer.Persistence.GenericRepository;
using Customer.Persistence.Persistence;
using Infrastructure.Common.Implementation;

namespace Customer.Persistence.UnitOfWork;

public class CustomerUnitOfWork : UnitOfWork<CustomerContext>, ICustomerUnitOfWork
{
    private ICustomerGenericRepository<CustomerSegment, Guid>? _customerSegment;
    private readonly CustomerContext _context;

    public CustomerUnitOfWork(CustomerContext context) : base(context)
    {
        _context = context;
    }

    public ICustomerGenericRepository<CustomerSegment, Guid> CustomerSegment =>
        _customerSegment ??= new CustomerGenericRepository<CustomerSegment, Guid>(_context);
}