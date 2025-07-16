using Microsoft.AspNetCore.Mvc;

namespace Shared.Utils.Params;

public abstract class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    [FromQuery(Name = "page-number")]
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    [FromQuery(Name = "page-size")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }
}