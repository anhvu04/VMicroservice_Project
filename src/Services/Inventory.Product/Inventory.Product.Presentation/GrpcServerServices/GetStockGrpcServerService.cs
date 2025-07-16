using Grpc.Core;
using Inventory.Product.API.Protos;
using Inventory.Product.Application.Usecases.InventoryEntry.Query.GetStockByProductId;
using MediatR;

namespace Inventory.Product.Presentation.GrpcServerServices;

public class GetStockGrpcServerService : StockProtoService.StockProtoServiceBase
{
    private readonly ISender _sender;

    public GetStockGrpcServerService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var res = await _sender.Send(new GetStockByProductIdQuery(Guid.Parse(request.ProductId)),
            context.CancellationToken);
        if (!res.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.NotFound, res.Error!));
        }

        return new GetStockResponse { Stock = res.Value!.Quantity };
    }
}