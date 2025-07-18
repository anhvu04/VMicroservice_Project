using Basket.Application.Abstractions;
using Product.Presentation.Grpc.Protos;
using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;

namespace Basket.Infrastructure.Grpc.Clients;

public class GetListProductsGrpcClientService : ICatalogProductService
{
    private readonly ProductProtoService.ProductProtoServiceClient _productProtoServiceClient;

    public GetListProductsGrpcClientService(ProductProtoService.ProductProtoServiceClient productProtoServiceClient)
    {
        _productProtoServiceClient = productProtoServiceClient;
    }

    public async Task<List<GetListCatalogProductsByIdResponse>> GetListCatalogProductsByIdAsync(
        GetListCatalogProductsByIdRequest byIdRequest)
    {
        try
        {
            var response = await _productProtoServiceClient.GetListProductsAsync(new GetListProductsRequest
            {
                Ids = { byIdRequest.Ids.Select(x => x.ToString()) }
            });

            return response.Products.Select(x => new GetListCatalogProductsByIdResponse()
            {
                Id = Guid.Parse(x.Id),
                Name = x.Name,
                OriginalPrice = x.OriginalPrice,
                SalePrice = x.SalePrice,
                Thumbnail = x.ThumbNail
            }).ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}