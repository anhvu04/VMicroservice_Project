namespace Product.Services.Utils.Pagination;

public class PaginationParams
{
    private string _searchTerm = "";
    private string _orderBy = "";
    private const int MaxPageSize = 10;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public string SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = string.IsNullOrEmpty(value) ? string.Empty : value.ToLower();
    }

    public string OrderBy
    {
        get => _orderBy;
        set => _orderBy = string.IsNullOrEmpty(value) ? string.Empty : value.ToLower();
    }

    public bool IsDescending { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }

    public bool HasNextPage { get; set; }
}