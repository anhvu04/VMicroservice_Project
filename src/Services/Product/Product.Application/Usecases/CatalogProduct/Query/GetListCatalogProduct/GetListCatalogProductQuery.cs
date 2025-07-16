using Contracts.Common.Interfaces.MediatR;
using Product.Application.Usecases.CatalogProduct.Common;
using Shared.Utils;
using Shared.Utils.Params;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProduct;

public class GetListCatalogProductQuery : BaseQuery, IQuery<PaginationResult<GetCatalogProductResponse>>
{
}