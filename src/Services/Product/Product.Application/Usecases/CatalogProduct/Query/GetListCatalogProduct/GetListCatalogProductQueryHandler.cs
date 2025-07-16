using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using Product.Application.Usecases.CatalogProduct.Common;
using Product.Domain.UnitOfWork;
using Shared.Utils;
using Shared.Utils.Params;

namespace Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProduct;

public class
    GetListCatalogProductQueryHandler : IQueryHandler<GetListCatalogProductQuery,
    PaginationResult<GetCatalogProductResponse>>
{
    private readonly IProductUnitOfWork _productUnitOfWork;

    public GetListCatalogProductQueryHandler(IProductUnitOfWork productUnitOfWork)
    {
        _productUnitOfWork = productUnitOfWork;
    }


    public async Task<Result<PaginationResult<GetCatalogProductResponse>>> Handle(GetListCatalogProductQuery request,
        CancellationToken cancellationToken)
    {
        var query = _productUnitOfWork.CatalogProduct.FindAll();
        Expression<Func<Domain.Entities.CatalogProduct, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            predicate = predicate.CombineAndAlsoExpressions(x => x.Name.Contains(request.SearchTerm));
        }

        query = query.Where(predicate);
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = ApplySorting(query, request);
        }

        var res =
            await query.ProjectToPaginatedListAsync<Domain.Entities.CatalogProduct, GetCatalogProductResponse>(request);
        return Result.Success(res);
    }

    #region Private Methods

    private static IQueryable<Domain.Entities.CatalogProduct> ApplySorting(
        IQueryable<Domain.Entities.CatalogProduct> query,
        BaseQuery paginationParams)
    {
        var orderBy = paginationParams.OrderBy!;
        var isDescending = paginationParams.IsDescending;
        return orderBy.ToLower().Replace(" ", "") switch
        {
            _ => query.ApplySorting(isDescending, Domain.Entities.CatalogProduct.GetSortValue(orderBy))
        };
    }

    #endregion
}