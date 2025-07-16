using Contracts.Common.Interfaces.MediatR;
using Customer.Application.Usecases.CustomerSegment.Common;
using Shared.Utils;
using Shared.Utils.Params;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetListCustomerSegment;

public class GetListCustomerSegmentQuery : BaseQuery, IQuery<PaginationResult<GetCustomerSegmentResponse>>
{
}