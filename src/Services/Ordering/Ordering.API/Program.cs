using Common.Logging;
using Ordering.API.Extensions;
using Ordering.Persistence.Persistence;
using Serilog;

namespace Ordering.API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);

        Log.Information("Starting Ordering API Up");

        try
        {
            // Add configurations to the container.
            builder.AddAppConfigurations();

            // Add infrastructure
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();
            // Use infrastructure
            app.UseInfrastructure();

            // Migrate database
            app.MigrateDatabase<OrderingDbContext>(); // muse be after builder.Build(); to prevent "The logger is already frozen"
            
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
            Log.Information("Ordering API Up Shutdown");
            Log.CloseAndFlush();
        }
    }
}