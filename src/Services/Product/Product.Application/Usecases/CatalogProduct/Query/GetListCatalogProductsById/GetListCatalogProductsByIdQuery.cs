using Contracts.Common.Interfaces.MediatR;
using Product.Application.Usecases.CatalogProduct.Common;
using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProductsById;

public class GetListCatalogProductsByIdQuery : GetListCatalogProductsByIdRequest,
    IQuery<List<GetListCatalogProductsByIdResponse>>
{
}