using Infrastructure.Middlewares;

namespace Basket.API.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
    }
}