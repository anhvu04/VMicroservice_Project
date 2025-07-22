using Customer.Domain.UnitOfWork;
using MapsterMapper;
using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;
using Shared.MediatR;
using Shared.Utils;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentByIdGrpc;

public class GetCustomerSegmentInfoByIdGrpcQueryHandler : IQueryHandler<GetCustomerSegmentInfoGrpcBaseRequest,
    GetCustomerSegmentInfoGrpcBaseResponse>
{
    private readonly ICustomerUnitOfWork _customerUnitOfWork;
    private readonly IMapper _mapper;

    public GetCustomerSegmentInfoByIdGrpcQueryHandler(ICustomerUnitOfWork customerUnitOfWork, IMapper mapper)
    {
        _customerUnitOfWork = customerUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCustomerSegmentInfoGrpcBaseResponse>> Handle(
        GetCustomerSegmentInfoGrpcBaseRequest request,
        CancellationToken cancellationToken)
    {
        var customerSegment =
            await _customerUnitOfWork.CustomerSegment.FindByIdAsync(request.CustomerId,
                cancellationToken: cancellationToken);
        if (customerSegment == null)
        {
            return Result.Failure<GetCustomerSegmentInfoGrpcBaseResponse>(
                $"{nameof(CustomerSegment)} with id: {request.CustomerId} does not exist");
        }

        return Result.Success(_mapper.Map<GetCustomerSegmentInfoGrpcBaseResponse>(customerSegment));
    }
}