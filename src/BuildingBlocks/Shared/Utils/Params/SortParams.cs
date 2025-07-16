using Microsoft.AspNetCore.Mvc;

namespace PRN232.Lab03.Services.Utils.Params;

public interface ISortParams
{
    [FromQuery(Name = "order-by")] public string? OrderBy { get; set; }

    [FromQuery(Name = "is-descending")] public bool IsDescending { get; set; }
}