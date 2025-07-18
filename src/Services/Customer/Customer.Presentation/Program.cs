using Common.Logging;
using Customer.Presentation.Controllers;
using Customer.Presentation.Extensions;
using Serilog;

namespace Customer.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting Customer API Up");

        try
        {
            // Add infrastructure
            builder.AddInfrastructure();

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
            Log.Information("Customer API Shutdown");
            Log.CloseAndFlush();
        }
    }
}