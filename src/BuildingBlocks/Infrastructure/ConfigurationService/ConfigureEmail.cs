using Contracts.Services.EmailService;
using Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.ConfigurationSettings;

namespace Infrastructure.ConfigurationService;

public static class ConfigureEmail
{
    public static void ConfigureEmailService(this WebApplicationBuilder builder)
    {
        _ = builder.Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ??
            throw new Exception("EmailSettings is not configured properly");
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
        builder.Services.AddSingleton<ISmtpEmailService, SmtpEmailService>();
    }
}