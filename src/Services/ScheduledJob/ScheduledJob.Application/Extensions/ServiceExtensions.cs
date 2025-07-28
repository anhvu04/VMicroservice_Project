using Infrastructure.ConfigurationService;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduledJob.Application.Common.HangfireJob;

namespace ScheduledJob.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
        services.ConfigureMapper();
        services.ConfigureHangfireJob();
    }

    private static void ConfigureHangfireJob(this IServiceCollection services)
    {
        services.AddScoped<SendCartNotificationScheduleJob>();
    }
}