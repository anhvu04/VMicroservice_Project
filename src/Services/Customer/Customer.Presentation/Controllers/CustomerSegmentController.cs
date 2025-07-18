using Customer.Application.Usecases.CustomerSegment.Query.GetCustomerSegmentById;
using Customer.Application.Usecases.CustomerSegment.Query.GetListCustomerSegments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Customer.Presentation.Controllers;

public static class CustomerSegmentController
{
    public static void MapCustomerSegmentController(this WebApplication app)
    {
        app.MapGet("/v1/customer-segments",
            [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
            async (HttpRequest request, ISender sender) =>
            {
                var query = new GetListCustomerSegmentsQuery
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

                var result = await sender.Send(query);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result);
            });

        app.MapGet("/v1/customer-segments/{id}",
            [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
            async ([FromRoute] Guid id, ISender sender) =>
            {
                var query = new GetCustomerSegmentByIdQuery(id);
                var result = await sender.Send(query);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result);
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