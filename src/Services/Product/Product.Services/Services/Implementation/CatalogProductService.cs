using System.Linq.Expressions;
using MapsterMapper;
using Product.Repositories.Entities;
using Product.Repositories.UnitOfWork;
using Product.Services.Models.Request.CatalogProduct;
using Product.Services.Models.Response.CatalogProduct;
using Product.Services.Services.Interfaces;
using Product.Services.Utils;
using Product.Services.Utils.Pagination;

namespace Product.Services.Services.Implementation;

public class CatalogProductService : ICatalogProductService
{
    private readonly IProductUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CatalogProductService(IProductUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResult<GetCatalogProduct>>> GetCatalogProductsAsync(
        GetCatalogProductsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.CatalogProduct.FindAll();
        Expression<Func<CatalogProduct, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            predicate = predicate.CombineAndAlsoExpressions(x => x.Name.Contains(request.SearchTerm));
        }

        query = query.Where(predicate);
        query = ApplySorting(query, request);
        var res = await query.ProjectToPaginatedListAsync<CatalogProduct, GetCatalogProduct>(request);
        return Result.Success(res);
    }

    public async Task<Result<GetCatalogProduct>> GetCatalogProductAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CatalogProduct.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure<GetCatalogProduct>($"{nameof(CatalogProduct)} with id: {id} does not exist");
        }

        return Result.Success(_mapper.Map<GetCatalogProduct>(entity));
    }

    public async Task<Result> CreateCatalogProductAsync(CreateCatalogProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CatalogProduct>(request);
        _unitOfWork.CatalogProduct.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateCatalogProductAsync(Guid id, UpdateCatalogProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CatalogProduct.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure($"{nameof(CatalogProduct)} with id: {id} does not exist");
        }

        _mapper.Map(request, entity);
        _unitOfWork.CatalogProduct.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteCatalogProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.CatalogProduct.FindByIdAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure($"{nameof(CatalogProduct)} with id: {id} does not exist");
        }

        _unitOfWork.CatalogProduct.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    #region Private Methods

    private static IQueryable<CatalogProduct> ApplySorting(IQueryable<CatalogProduct> query,
        PaginationParams paginationParams)
    {
        var orderBy = paginationParams.OrderBy;
        var isDescending = paginationParams.IsDescending;
        return orderBy.ToLower().Replace(" ", "") switch
        {
            _ => query.ApplySorting(isDescending, CatalogProduct.GetSortValue(orderBy))
        };
    }

    #endregion
}