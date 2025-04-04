using Customer.Services.Models.Requests.CustomerSegment;
using Customer.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers;

public static class CustomerSegmentController
{
    public static void MapCustomerSegmentController(this WebApplication app)
    {
        app.MapGet("/api/customer-segment",
            async (HttpRequest request, ICustomerSegmentService service) =>
            {
                var requestParams = new GetCustomerSegmentsRequest
                {
                    SearchTerm = request.Query["searchTerm"]!,
                    OrderBy = request.Query["orderBy"]!,
                    PageSize = string.IsNullOrEmpty(request.Query["pageSize"])
                        ? 10
                        : int.Parse(request.Query["pageSize"]!),
                    PageNumber = string.IsNullOrEmpty(request.Query["pageNumber"])
                        ? 1
                        : int.Parse(request.Query["pageNumber"]!),
                    IsDescending = !string.IsNullOrEmpty(request.Query["isDescending"]) &&
                                   bool.Parse(request.Query["isDescending"]!),
                };

                var res = await service.GetCustomerSegmentsAsync(requestParams);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.BadRequest(res);
            });

        app.MapGet("/api/customer-segment/{id}",
            async ([FromRoute] Guid id, ICustomerSegmentService service) =>
            {
                var res = await service.GetCustomerSegmentAsync(id);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.BadRequest(res);
            });

        app.MapPost("/api/customer-segment",
            async ([FromBody] CreateCustomerSegmentRequest request, ICustomerSegmentService service) =>
            {
                var res = await service.CreateCustomerSegmentAsync(request);
                return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
            });

        app.MapPatch("/api/customer-segment/{id}",
            async ([FromRoute] Guid id, [FromBody] UpdateCustomerSegmentRequest request,
                ICustomerSegmentService service) =>
            {
                var res = await service.UpdateCustomerSegmentAsync(id, request);
                return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
            });

        app.MapDelete("/api/customer-segment/{id}",
            async ([FromRoute] Guid id, ICustomerSegmentService service) =>
            {
                var res = await service.DeleteCustomerSegmentAsync(id);
                return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
            });
    }
}