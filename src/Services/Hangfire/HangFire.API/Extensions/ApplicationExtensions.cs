using Hangfire;
using Infrastructure.ConfigurationService;
using Infrastructure.Middlewares;

namespace HangFire.API.Extensions;

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
        app.ConfigureHangFireDashboard(app.Configuration);
    }
}