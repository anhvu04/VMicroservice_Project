using Customer.Presentation.Controllers;
using Customer.Persistence.Extensions;
using Customer.Persistence.Persistence;
using Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Customer.Presentation.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        // Map Controllers
        app.MapCustomerSegmentController();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
        app.MigrateDatabase<CustomerContext>();
    }
}