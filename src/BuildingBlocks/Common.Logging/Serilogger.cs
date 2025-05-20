using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Common.Logging;

public static class Serilogger
{
    public static readonly Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger = (context, configuration) =>
    {
        var applicationName = context.HostingEnvironment.ApplicationName.ToLower().Replace(".", "-");
        var environmentName = context.HostingEnvironment.EnvironmentName;
        configuration.WriteTo.Debug().WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate)
            .Enrich.FromLogContext().Enrich.WithMachineName()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("Environment", environmentName)
            .ReadFrom.Configuration(context.Configuration);
    };
}