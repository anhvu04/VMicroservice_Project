using Infrastructure.Middlewares;
using Ocelot.Middleware;

namespace OcelotApiGateway.Extensions;

public static class ApplicationExtensions
{
    public static async Task UseInfrastructure(this WebApplication app)
    {
        app.UseCors("CorsPolicy");
        app.UseMiddleware();
        app.UseSwaggerOcelot();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
        await app.UseOcelot();
    }

    private static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    private static void UseSwaggerOcelot(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerForOcelotUI(opt => { opt.PathToSwaggerGenerator = "/swagger/docs"; });
    }

    // private static void UseSwagger(this WebApplication app)
    // {
    //     app.UseSwaggerUI();
    // }
}