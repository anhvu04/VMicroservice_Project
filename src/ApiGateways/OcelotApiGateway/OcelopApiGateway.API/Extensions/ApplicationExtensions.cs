using Infrastructure.Middlewares;
using Ocelot.Middleware;

namespace OcelotApiGateway.Extensions;

public static class ApplicationExtensions
{
    public static async Task UseInfrastructure(this WebApplication app)
    {
        app.UseCors("CorsPolicy");
        app.ConfigureMiddleware();
        app.ConfigureSwagger();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
        await app.UseOcelot();
    }

    private static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    private static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}