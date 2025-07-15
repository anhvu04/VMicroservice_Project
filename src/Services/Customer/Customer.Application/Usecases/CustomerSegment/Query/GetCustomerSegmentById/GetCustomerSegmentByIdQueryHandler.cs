using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using Customer.Application.Usecases.CustomerSegment.Common;
using Customer.Domain.UnitOfWork;
using MapsterMapper;
using Shared.Utils;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentById;

public class GetCustomerSegmentByIdQueryHandler : IQueryHandler<GetCustomerSegmentByIdQuery, GetCustomerSegmentResponse>
{
    private readonly ICustomerUnitOfWork _customerUnitOfWork;
    private readonly IMapper _mapper;

    public GetCustomerSegmentByIdQueryHandler(ICustomerUnitOfWork customerUnitOfWork, IMapper mapper)
    {
        _customerUnitOfWork = customerUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCustomerSegmentResponse>> Handle(GetCustomerSegmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customerSegment =
            await _customerUnitOfWork.CustomerSegment.FindByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (customerSegment == null)
        {
            return Result.Failure<GetCustomerSegmentResponse>($"{nameof(CustomerSegment)} with id: {request.Id} does not exist");
        }

        return Result.Success(_mapper.Map<GetCustomerSegmentResponse>(customerSegment));
    }
}