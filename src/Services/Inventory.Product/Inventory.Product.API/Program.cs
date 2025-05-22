using Common.Logging;
using Inventory.Product.API.Extensions;
using Serilog;

namespace Inventory.Product.API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting Inventory Product API Up");

        try
        {
            // Add configurations to the container.
            builder.AddAppConfigurations();

            // Add infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseInfrastructure();
            app.MigrateDatabase();

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
            Log.Information("Inventory Product API Up Shutdown");
            Log.CloseAndFlush();
        }
    }
}