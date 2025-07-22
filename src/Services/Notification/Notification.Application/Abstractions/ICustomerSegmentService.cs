using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;

namespace Notification.Application.Abstractions;

public interface ICustomerSegmentService
{
    Task<GetCustomerSegmentInfoGrpcBaseResponse> GetCustomerSegmentInfoAsync(Guid customerId);
}