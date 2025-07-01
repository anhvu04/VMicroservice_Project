using Customer.Services.Models.Requests.CustomerSegment;
using Customer.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Customer.API.Controllers;

public static class CustomerSegmentController
{
    public static void MapCustomerSegmentController(this WebApplication app)
    {
        app.MapGet("/v1/customer-segments",
            [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
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

        app.MapGet("/v1/customer-segments/{id}",
            [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
            async ([FromRoute] Guid id, ICustomerSegmentService service) =>
            {
                var res = await service.GetCustomerSegmentAsync(id);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.BadRequest(res);
            });

        // app.MapPost("/v1/customer-segments",
        //     async ([FromBody] CreateCustomerSegmentRequest request, ICustomerSegmentService service) =>
        //     {
        //         var res = await service.CreateCustomerSegmentAsync(request);
        //         return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
        //     });
        //
        //
        // app.MapPatch("/v1/customer-segments/{id}",
        //     [Authorize(Roles = nameof(UserRoles.Admin))]
        //     async ([FromRoute] Guid id, [FromBody] UpdateCustomerSegmentRequest request,
        //         ICustomerSegmentService service) =>
        //     {
        //         var res = await service.UpdateCustomerSegmentAsync(id, request);
        //         return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
        //     });
        //
        // app.MapDelete("/v1/customer-segments/{id}",
        //     [Authorize(Roles = nameof(UserRoles.Admin))]
        //     async ([FromRoute] Guid id, ICustomerSegmentService service) =>
        //     {
        //         var res = await service.DeleteCustomerSegmentAsync(id);
        //         return res.IsSuccess ? Results.Ok() : Results.BadRequest(res);
        //     });
    }
}