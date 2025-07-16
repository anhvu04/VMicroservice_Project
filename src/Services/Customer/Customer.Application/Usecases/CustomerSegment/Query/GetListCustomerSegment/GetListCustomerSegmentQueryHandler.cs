using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using Customer.Application.Usecases.CustomerSegment.Common;
using Customer.Domain.UnitOfWork;
using Shared.Utils;
using Shared.Utils.Params;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetListCustomerSegment;

public class
    GetListCustomerSegmentQueryHandler : IQueryHandler<GetListCustomerSegmentQuery,
    PaginationResult<GetCustomerSegmentResponse>>
{
    private readonly ICustomerUnitOfWork _customerUnitOfWork;

    public GetListCustomerSegmentQueryHandler(ICustomerUnitOfWork customerUnitOfWork)
    {
        _customerUnitOfWork = customerUnitOfWork;
    }

    public async Task<Result<PaginationResult<GetCustomerSegmentResponse>>> Handle(GetListCustomerSegmentQuery request,
        CancellationToken cancellationToken)
    {
        var query = _customerUnitOfWork.CustomerSegment.FindAll();
        Expression<Func<Domain.Entities.CustomerSegment, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            predicate = predicate.CombineAndAlsoExpressions(x => x.FirstName.Contains(request.SearchTerm) ||
                                                                 x.LastName.Contains(request.SearchTerm));
        }

        query = query.Where(predicate);
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = ApplySorting(query, request);
        }

        var res =
            await query.ProjectToPaginatedListAsync<Domain.Entities.CustomerSegment, GetCustomerSegmentResponse>(request);
        return Result.Success(res);
    }

    #region Private Methods

    private static IQueryable<Domain.Entities.CustomerSegment> ApplySorting(
        IQueryable<Domain.Entities.CustomerSegment> query,
        BaseQuery baseQuery)
    {
        var orderBy = baseQuery.OrderBy!;
        var isDescending = baseQuery.IsDescending;
        return orderBy.ToLower().Replace(" ", "") switch
        {
            _ => query.ApplySorting(isDescending, Domain.Entities.CustomerSegment.GetSortValue(orderBy))
        };
    }

    #endregion
}