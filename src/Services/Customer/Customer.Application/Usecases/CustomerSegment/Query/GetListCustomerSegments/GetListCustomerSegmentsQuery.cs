using Customer.Application.Usecases.CustomerSegment.Common;
using Shared.MediatR;
using Shared.Utils;
using Shared.Utils.Params;

namespace Customer.Application.Usecases.CustomerSegment.Query.GetListCustomerSegments;

public class GetListCustomerSegmentsQuery : BaseQuery, IQuery<PaginationResult<GetCustomerSegmentResponse>>
{
}