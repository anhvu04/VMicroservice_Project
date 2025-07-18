using Common.Logging;
using Product.Presentation.Extensions;
using Serilog;

namespace Product.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting Product API Up");

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
            Log.Information("Product API Shutdown");
            Log.CloseAndFlush();
        }
    }
}