using Basket.API.Extensions;
using Common.Logging;
using Serilog;

namespace Basket.API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting Basket API Up");

        try
        {
            // Add configurations to the container.
            builder.AddAppConfigurations();

            // Add infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();
            
            // Use infrastructure
            app.UseInfrastructure();
            app.Run();
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
            Log.Information("Basket API Up Shutdown");
            Log.CloseAndFlush();
        }
    }
}