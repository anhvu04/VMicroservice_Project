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
        services.ConfigureMapper();
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
        services.ConfigureHangfireJob();
    }

    private static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }

    private static void ConfigureHangfireJob(this IServiceCollection services)
    {
        services.AddScoped<SendCartNotificationScheduleJob>();
    }
}