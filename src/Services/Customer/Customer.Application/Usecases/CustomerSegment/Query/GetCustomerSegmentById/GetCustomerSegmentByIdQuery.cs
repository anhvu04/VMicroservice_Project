using Contracts.Common.Interfaces.MediatR;
using Customer.Application.Usecases.CustomerSegment.Common;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentById;

public class GetCustomerSegmentByIdQuery(Guid id) : IQuery<GetCustomerSegmentResponse>
{
    public Guid Id { get; set; } = id;
}