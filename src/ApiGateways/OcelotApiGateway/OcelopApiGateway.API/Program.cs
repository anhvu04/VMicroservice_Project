using Common.Logging;
using OcelotApiGateway.Extensions;
using Serilog;

namespace OcelotApiGateway;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting API Gateway Up");


        try
        {
            // Add Infrastructure
            builder.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Use Infrastructure
            await app.UseInfrastructure();

            await app.RunAsync();
        }
        catch (Exception e)
        {
            string type = e.GetType().Name;
            if (type.Equals("StopTheHostException", StringComparison.Ordinal))
            {
                throw;
            }

            Log.Fatal(e, "Unhandled exception");
        }
        finally
        {
            Log.Information("API Gateway Shutdown");
            Log.CloseAndFlush();
        }
    }
}