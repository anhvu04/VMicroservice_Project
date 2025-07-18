using Hangfire;
using Infrastructure.ConfigurationService;
using Infrastructure.Middlewares;
using ScheduledJob.Presentation.Grpc.Servers;

namespace ScheduledJob.Presentation.Extensions;

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
        app.MapGrpcService<CartNotificationGrpcServer>();
    }
}