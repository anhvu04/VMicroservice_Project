using Basket.Application.Abstractions;
using Inventory.Product.Presentation.Grpc.Protos;

namespace Basket.Infrastructure.Grpc.Clients;

public class GetInventoryEntryGrpcClientService : IInventoryEntryService
{
    private readonly InventoryEntryProtoService.InventoryEntryProtoServiceClient _inventoryEntryProtoServiceClient;

    public GetInventoryEntryGrpcClientService(InventoryEntryProtoService.InventoryEntryProtoServiceClient inventoryEntryProtoServiceClient)
    {
        _inventoryEntryProtoServiceClient = inventoryEntryProtoServiceClient;
    }


    public async Task<int> GetStockAsync(string productId)
    {
        var stock = await _inventoryEntryProtoServiceClient.GetStockAsync(new GetStockRequest { ProductId = productId });
        return stock.Stock;
    }
}