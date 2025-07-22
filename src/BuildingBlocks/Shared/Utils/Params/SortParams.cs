using Microsoft.AspNetCore.Mvc;

namespace Shared.Utils.Params;

public interface ISortParams
{
    [FromQuery(Name = "order-by")] public string? OrderBy { get; set; }

    [FromQuery(Name = "is-descending")] public bool IsDescending { get; set; }
}