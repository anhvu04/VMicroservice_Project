using Shared.MediatR;

namespace Shared.InfrastructureGrpcModels.CustomerSegmentInfo;

public class GetCustomerSegmentInfoGrpcBaseRequest(Guid customerId) : IQuery<GetCustomerSegmentInfoGrpcBaseResponse>
{ 
    public Guid CustomerId { get; set; } = customerId;
}