using Notification.Application.Abstractions;
using Product.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.Utils.Errors;

namespace Notification.Infrastructure.Grpc.Clients;

public class ListProductsGrpcClient : ICatalogProductService
{
    private readonly ProductProtoService.ProductProtoServiceClient _productProtoServiceClient;

    public ListProductsGrpcClient(ProductProtoService.ProductProtoServiceClient productProtoServiceClient)
    {
        _productProtoServiceClient = productProtoServiceClient;
    }

    public async Task<List<GetListCatalogProductsByIdGrpcBaseResponse>> GetListCatalogProductsByIdAsync(
        GetListCatalogProductsByIdGrpcBaseRequest byIdGrpcBaseRequest)
    {
        try
        {
            var response = await _productProtoServiceClient.GetListProductsAsync(new GetListProductsRequest
            {
                Ids = { byIdGrpcBaseRequest.Ids.Select(x => x.ToString()) }
            });

            return response.Products.Select(x => new GetListCatalogProductsByIdGrpcBaseResponse()
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
            throw new Exception(GrpcCalledError.BasketClientError.GetListCatalogProductsError, e);
        }
    }
}