namespace Product.Services.Utils.Pagination;

public class PaginationResult<T>
{
    public IReadOnlyCollection<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public PaginationResult(IReadOnlyCollection<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        HasPreviousPage = pageNumber > 1;
        HasNextPage = pageNumber < TotalPages;
    }
}