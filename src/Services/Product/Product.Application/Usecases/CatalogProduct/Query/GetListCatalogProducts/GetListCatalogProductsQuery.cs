using Product.Application.Usecases.CatalogProduct.Common;
using Shared.MediatR;
using Shared.Utils;
using Shared.Utils.Params;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProducts;

public class GetListCatalogProductsQuery : BaseQuery, IQuery<PaginationResult<GetCatalogProductResponse>>
{
}