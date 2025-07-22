using Microsoft.AspNetCore.Mvc;

namespace Shared.Utils.Params;

public interface ISearchParams
{
    [FromQuery(Name = "search-term")] public string? SearchTerm { get; set; }
}