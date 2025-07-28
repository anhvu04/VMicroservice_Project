using Identity.Presentation.Grpc.Protos;
using Microsoft.Extensions.Logging;
using Notification.Application.Abstractions;
using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;
using Shared.Utils;
using Shared.Utils.Errors;

namespace Notification.Infrastructure.Grpc.Clients;

public class CustomerInfoGrpcClient : ICustomerSegmentService
{
    private readonly CustomerInfoProtoService.CustomerInfoProtoServiceClient _customerInfoProtoServiceClient;
    private readonly ILogger<CustomerInfoGrpcClient> _logger;

    public CustomerInfoGrpcClient(
        CustomerInfoProtoService.CustomerInfoProtoServiceClient customerInfoProtoServiceClient,
        ILogger<CustomerInfoGrpcClient> logger)
    {
        _customerInfoProtoServiceClient = customerInfoProtoServiceClient;
        _logger = logger;
    }

    public async Task<Result<GetCustomerSegmentInfoGrpcBaseResponse>> GetCustomerSegmentInfoAsync(Guid customerId)
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
            _logger.LogError(e, GrpcCalledError.NotificationClientError.GetCustomerInfoError);
            return Result.Failure<GetCustomerSegmentInfoGrpcBaseResponse>(GrpcCalledError.NotificationClientError
                .GetCustomerInfoError);
        }
    }
}