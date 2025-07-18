using Contracts.Common.Interfaces.MediatR;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Product.Application.Usecases.CatalogProduct.Common;
using Product.Domain.UnitOfWork;
using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProductsById;

public class
    GetListCatalogProductsByIdQueryHandler : IQueryHandler<GetListCatalogProductsByIdQuery,
    List<GetListCatalogProductsByIdResponse>>
{
    private readonly IProductUnitOfWork _productUnitOfWork;

    public GetListCatalogProductsByIdQueryHandler(IProductUnitOfWork productUnitOfWork)
    {
        _productUnitOfWork = productUnitOfWork;
    }

    public async Task<Result<List<GetListCatalogProductsByIdResponse>>> Handle(GetListCatalogProductsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productUnitOfWork.CatalogProduct.FindAll(x => request.Ids.Contains(x.Id),
                cancellationToken: cancellationToken).ProjectToType<GetListCatalogProductsByIdResponse>()
            .ToListAsync(cancellationToken: cancellationToken);
        return products;
    }
}