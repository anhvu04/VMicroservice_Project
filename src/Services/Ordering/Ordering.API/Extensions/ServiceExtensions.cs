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
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructures(builder.Configuration);
        builder.AddPersistence();
        builder.ConfigureEmailService();
    }

    private static void ConfigureEmailService(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
        builder.Services.AddSingleton<ISmtpEmailService, SmtpEmailService>();
    }
}