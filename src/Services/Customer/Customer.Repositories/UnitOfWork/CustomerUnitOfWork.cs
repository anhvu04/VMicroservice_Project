using Customer.Repositories.Entities;
using Customer.Repositories.GenericRepository;
using Customer.Repositories.Persistence;
using Infrastructure.Common.Implementation;

namespace Customer.Repositories.UnitOfWork;

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