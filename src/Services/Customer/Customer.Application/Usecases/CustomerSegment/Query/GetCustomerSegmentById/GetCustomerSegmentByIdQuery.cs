using Customer.Application.Usecases.CustomerSegment.Common;
using Shared.MediatR;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentById;

public class GetCustomerSegmentByIdQuery(Guid id) : IQuery<GetCustomerSegmentResponse>
{
    public Guid Id { get; set; } = id;
}