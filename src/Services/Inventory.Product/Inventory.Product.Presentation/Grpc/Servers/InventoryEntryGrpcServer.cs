using Grpc.Core;
using Inventory.Product.Application.Usecases.InventoryEntry.Query.GetStockByProductId;
using Inventory.Product.Presentation.Grpc.Protos;
using MediatR;

namespace Inventory.Product.Presentation.Grpc.Servers;

public class InventoryEntryGrpcServer : InventoryEntryProtoService.InventoryEntryProtoServiceBase
{
    private readonly ISender _sender;

    public InventoryEntryGrpcServer(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var res = await _sender.Send(new GetStockByProductIdQuery(Guid.Parse(request.ProductId)),
            context.CancellationToken);
        if (!res.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, res.Error!));
        }

        return new GetStockResponse { Stock = res.Value!.Quantity };
    }
}