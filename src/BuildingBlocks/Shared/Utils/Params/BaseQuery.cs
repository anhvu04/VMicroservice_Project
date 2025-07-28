namespace Shared.Utils.Params;

public class BaseQuery : PaginationParams, ISearchParams, ISortParams
{
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}