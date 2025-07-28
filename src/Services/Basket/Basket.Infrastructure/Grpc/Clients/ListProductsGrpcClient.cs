using Basket.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Product.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.Utils;
using Shared.Utils.Errors;

namespace Basket.Infrastructure.Grpc.Clients;

public class ListProductsGrpcClient : ICatalogProductService
{
    private readonly ProductProtoService.ProductProtoServiceClient _productProtoServiceClient;
    private readonly ILogger<ListProductsGrpcClient> _logger;

    public ListProductsGrpcClient(ProductProtoService.ProductProtoServiceClient productProtoServiceClient,
        ILogger<ListProductsGrpcClient> logger)
    {
        _productProtoServiceClient = productProtoServiceClient;
        _logger = logger;
    }

    public async Task<Result<List<GetListCatalogProductsByIdGrpcBaseResponse>>> GetListCatalogProductsByIdAsync(
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
            _logger.LogError(e, GrpcCalledError.BasketClientError.GetListCatalogProductsError);
            return Result.Failure<List<GetListCatalogProductsByIdGrpcBaseResponse>>(GrpcCalledError.BasketClientError
                .GetListCatalogProductsError);
        }
    }
}