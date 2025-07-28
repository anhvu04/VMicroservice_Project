using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;
using Shared.Utils;

namespace Notification.Application.Abstractions;

public interface ICustomerSegmentService
{
    Task<Result<GetCustomerSegmentInfoGrpcBaseResponse>> GetCustomerSegmentInfoAsync(Guid customerId);
}