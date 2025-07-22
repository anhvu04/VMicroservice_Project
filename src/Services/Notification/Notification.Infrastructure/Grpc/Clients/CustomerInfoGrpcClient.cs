using Identity.Presentation.Grpc.Protos;
using Notification.Application.Abstractions;
using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;
using Shared.Utils.Errors;

namespace Notification.Infrastructure.Grpc.Clients;

public class CustomerInfoGrpcClient : ICustomerSegmentService
{
    private readonly CustomerInfoProtoService.CustomerInfoProtoServiceClient _customerInfoProtoServiceClient;

    public CustomerInfoGrpcClient(
        CustomerInfoProtoService.CustomerInfoProtoServiceClient customerInfoProtoServiceClient)
    {
        _customerInfoProtoServiceClient = customerInfoProtoServiceClient;
    }

    public async Task<GetCustomerSegmentInfoGrpcBaseResponse> GetCustomerSegmentInfoAsync(Guid customerId)
    {
        try
        {
            var response = await _customerInfoProtoServiceClient.GetCustomerInfoAsync(new GetCustomerInfoRequest
            {
                CustomerId = customerId.ToString()
            });

            return new GetCustomerSegmentInfoGrpcBaseResponse
            {
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName
            };
        }
        catch (Exception e)
        {
            throw new Exception(GrpcCalledError.NotificationClientError.GetCustomerInfoError, e);
        }
    }
}