using Common.Logging;
using HangFire.API.Extensions;
using Serilog;

namespace HangFire.API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog(Serilogger.ConfigureLogger);
        Log.Information("Starting HangFire API Up");

        try
        {
            // Add infrastructure
            builder.AddInfrastructure(builder.Configuration);

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
            Log.Information("HangFire API Shutdown");
            Log.CloseAndFlush();
        }
    }
}