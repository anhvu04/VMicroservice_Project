using Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentById;
using Grpc.Core;
using Identity.Presentation.Grpc.Protos;
using MediatR;
using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;

namespace Customer.Presentation.Grpc.Servers;

public class CustomerInfoGrpcServer : CustomerInfoProtoService.CustomerInfoProtoServiceBase
{
    private readonly ISender _sender;

    public CustomerInfoGrpcServer(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetCustomerInfoResponse> GetCustomerInfo(GetCustomerInfoRequest request,
        ServerCallContext context)
    {
        var res = await _sender.Send(new GetCustomerSegmentInfoGrpcBaseRequest(Guid.Parse(request.CustomerId)),
            context.CancellationToken);
        if (!res.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, res.Error!));
        }

        return new GetCustomerInfoResponse
        {
            FirstName = res.Value!.FirstName,
            LastName = res.Value.LastName,
            Email = res.Value.Email
        };
    }
}