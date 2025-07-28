using Mapster;
using Microsoft.EntityFrameworkCore;
using Product.Domain.UnitOfWork;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.MediatR;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProductsByIdGrpc;

public class
    GetListCatalogProductsByIdQueryHandler : IQueryHandler<GetListCatalogProductsByIdGrpcBaseRequest,
    List<GetListCatalogProductsByIdGrpcBaseResponse>>
{
    private readonly IProductUnitOfWork _productUnitOfWork;

    public GetListCatalogProductsByIdQueryHandler(IProductUnitOfWork productUnitOfWork)
    {
        _productUnitOfWork = productUnitOfWork;
    }

    public async Task<Result<List<GetListCatalogProductsByIdGrpcBaseResponse>>> Handle(
        GetListCatalogProductsByIdGrpcBaseRequest request,
        CancellationToken cancellationToken)
    {
        var products = await _productUnitOfWork.CatalogProduct.FindAll(x => request.Ids.Contains(x.Id),
                cancellationToken: cancellationToken).ProjectToType<GetListCatalogProductsByIdGrpcBaseResponse>()
            .ToListAsync(cancellationToken: cancellationToken);
        return products;
    }
}