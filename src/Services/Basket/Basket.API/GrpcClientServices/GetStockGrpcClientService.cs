using Basket.Services.Services.Interfaces;
using Inventory.Product.API.Protos;

namespace Basket.API.GrpcClientServices;

public class GetStockGrpcClientService : IStockService
{
    private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;

    public GetStockGrpcClientService(StockProtoService.StockProtoServiceClient stockProtoServiceClient)
    {
        _stockProtoServiceClient = stockProtoServiceClient;
    }

    public Task<int> GetStockAsync(string productId)
    {
        var stock = _stockProtoServiceClient.GetStock(new GetStockRequest { ProductId = productId });
        return Task.FromResult(stock.Stock);
    }
}