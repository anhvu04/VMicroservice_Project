using Grpc.Core;
using MediatR;
using Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProductsByIdGrpc;
using Product.Presentation.Grpc.Protos;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

namespace Product.Presentation.Grpc.Servers;

public class ProductGrpcServer : ProductProtoService.ProductProtoServiceBase
{
    private readonly ISender _sender;

    public ProductGrpcServer(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<GetListProductsResponse> GetListProducts(GetListProductsRequest request,
        ServerCallContext context)
    {
        var query = new GetListCatalogProductsByIdGrpcBaseRequest
        {
            Ids = request.Ids.Select(Guid.Parse).ToList()
        };
        var result = await _sender.Send(query, context.CancellationToken);
        if (!result.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, result.Error!));
        }

        return new GetListProductsResponse
        {
            Products =
            {
                result.Value!.Select(x => new ProductResponse
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    OriginalPrice = x.OriginalPrice,
                    SalePrice = x.SalePrice,
                    ThumbNail = x.Thumbnail
                })
            }
        };
    }
}