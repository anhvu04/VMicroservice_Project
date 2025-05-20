using Contracts.Services.EmailService;
using Infrastructure.Services.EmailService;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Ordering.Persistence.Extensions;
using Shared.ConfigurationSettings;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddEmailService(configuration);

        services.AddPersistence(configuration);
        services.AddApplication();
        services.AddInfrastructures(configuration);
    }

    private static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.AddSingleton<ISmtpEmailService, SmtpEmailService>();
    }
}