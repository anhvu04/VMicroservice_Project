using System.Linq.Expressions;
using MapsterMapper;
using Customer.Repositories.Entities;
using Customer.Repositories.UnitOfWork;
using Customer.Services.Models.Requests.CustomerSegment;
using Customer.Services.Models.Responses.CustomerSegment;
using Customer.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;
using Shared.Utils.Pagination;

namespace Customer.Services.Services.Implementation;

public class CustomerSegmentService : ICustomerSegmentService
{
    private readonly ICustomerUnitOfWork _unitOfWork;
    private readonly IPasswordHashing _passwordHashing;
    private readonly IMapper _mapper;

    public CustomerSegmentService(ICustomerUnitOfWork unitOfWork, IMapper mapper, IPasswordHashing passwordHashing)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHashing = passwordHashing;
    }

    public async Task<Result<PaginationResult<GetCustomerSegment>>> GetCustomerSegmentsAsync(
        GetCustomerSegmentsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.CustomerSegment.FindAll();
        Expression<Func<CustomerSegment, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            predicate = predicate.CombineAndAlsoExpressions(x => x.FirstName.Contains(request.SearchTerm) ||
                                                                 x.LastName.Contains(request.SearchTerm));
        }

        query = query.Where(predicate);
        query = ApplySorting(query, request);
        var res = await query.ProjectToPaginatedListAsync<CustomerSegment, GetCustomerSegment>(request);
        return Result.Success(res);
    }

    public async Task<Result<GetCustomerSegment>> GetCustomerSegmentAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var customerSegment = await _unitOfWork.CustomerSegment.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (customerSegment == null)
        {
            return Result.Failure<GetCustomerSegment>($"{nameof(CustomerSegment)} with id: {id} does not exist");
        }

        return Result.Success(_mapper.Map<GetCustomerSegment>(customerSegment));
    }

    public async Task<Result> CreateCustomerSegmentAsync(CreateCustomerSegmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var existCustomerSegment = await
            _unitOfWork.CustomerSegment.FindAll(cancellationToken: cancellationToken).Select(x => new
            {
                IsExistUserName = x.UserName == request.UserName,
                IsExistEmail = x.Email == request.Email,
                IsExistPhoneNumber = x.PhoneNumber == request.PhoneNumber
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (existCustomerSegment != null)
        {
            if (existCustomerSegment.IsExistUserName)
            {
                return Result.Failure("UserName is exist");
            }

            if (existCustomerSegment.IsExistEmail)
            {
                return Result.Failure("Email is exist");
            }

            if (existCustomerSegment.IsExistPhoneNumber)
            {
                return Result.Failure("PhoneNumber is exist");
            }
        }

        var customerSegment = _mapper.Map<CustomerSegment>(request);
        customerSegment.Password = await _passwordHashing.HashPasswordAsync(request.Password);
        _unitOfWork.CustomerSegment.Add(customerSegment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateCustomerSegmentAsync(Guid id, UpdateCustomerSegmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var customerSegment = await _unitOfWork.CustomerSegment.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (customerSegment == null)
        {
            return Result.Failure($"{nameof(CustomerSegment)} with id: {id} does not exist");
        }

        _mapper.Map(request, customerSegment);
        _unitOfWork.CustomerSegment.Update(customerSegment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteCustomerSegmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customerSegment = await _unitOfWork.CustomerSegment.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (customerSegment == null)
        {
            return Result.Failure($"{nameof(CustomerSegment)} with id: {id} does not exist");
        }

        _unitOfWork.CustomerSegment.Delete(customerSegment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    #region Private Methods

    private static IQueryable<CustomerSegment> ApplySorting(IQueryable<CustomerSegment> query,
        PaginationParams paginationParams)
    {
        var orderBy = paginationParams.OrderBy;
        var isDescending = paginationParams.IsDescending;
        return orderBy.ToLower().Replace(" ", "") switch
        {
            _ => query.ApplySorting(isDescending, CustomerSegment.GetSortValue(orderBy))
        };
    }

    #endregion
}