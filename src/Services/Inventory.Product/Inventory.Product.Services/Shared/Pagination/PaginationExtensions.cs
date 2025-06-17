using MongoDB.Driver;

namespace Inventory.Product.Services.Shared.Pagination;

public static class PaginationExtensions
{
    public static async Task<PaginationResult<T>> ToPaginatedListAsync<T>(
        this IFindFluent<T, T> findFluent,
        PaginationParams paginationParams)
    {
        var totalItems = await findFluent.CountDocumentsAsync();

        var items = await findFluent
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Limit(paginationParams.PageSize)
            .ToListAsync();

        return new PaginationResult<T>(
            items,
            (int)totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    // Pagination with mapping for IFindFluent
    public static async Task<PaginationResult<TResult>> ToPaginatedListAsync<T, TResult>(
        this IFindFluent<T, T> findFluent,
        PaginationParams paginationParams,
        Func<T, TResult> mapper)
    {
        var totalItems = await findFluent.CountDocumentsAsync();

        var items = await findFluent
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Limit(paginationParams.PageSize)
            .ToListAsync();

        var mappedItems = items.Select(mapper).ToList();

        return new PaginationResult<TResult>(
            mappedItems,
            (int)totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }
}