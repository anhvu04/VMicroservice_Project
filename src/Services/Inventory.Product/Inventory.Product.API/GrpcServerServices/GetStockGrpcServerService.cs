using Grpc.Core;
using Inventory.Product.API.Protos;
using Inventory.Product.Services.Services.Interfaces;

namespace Inventory.Product.API.GrpcServerServices;

public class GetStockGrpcServerService : StockProtoService.StockProtoServiceBase
{
    private readonly IInventoryEntryService _inventoryEntryService;

    public GetStockGrpcServerService(IInventoryEntryService inventoryEntryService)
    {
        _inventoryEntryService = inventoryEntryService;
    }

    public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var stock = await _inventoryEntryService.GetStock(request.ProductId);
        return new GetStockResponse { Stock = stock.Value };
    }
}