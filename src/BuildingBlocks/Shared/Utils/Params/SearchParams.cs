using Microsoft.AspNetCore.Mvc;

namespace PRN232.Lab03.Services.Utils.Params;

public interface ISearchParams
{
    [FromQuery(Name = "search-term")] public string? SearchTerm { get; set; }
}