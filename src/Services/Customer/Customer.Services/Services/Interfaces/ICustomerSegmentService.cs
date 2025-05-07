using Customer.Services.Models.Requests.CustomerSegment;
using Customer.Services.Models.Responses.CustomerSegment;
using Shared.Utils;
using Shared.Utils.Pagination;

namespace Customer.Services.Services.Interfaces;

public interface ICustomerSegmentService
{
    Task<Result<PaginationResult<GetCustomerSegment>>> GetCustomerSegmentsAsync(GetCustomerSegmentsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<GetCustomerSegment>>
        GetCustomerSegmentAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> CreateCustomerSegmentAsync(CreateCustomerSegmentRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateCustomerSegmentAsync(Guid id, UpdateCustomerSegmentRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteCustomerSegmentAsync(Guid id, CancellationToken cancellationToken = default);
}