using Contracts.Services.ScheduledJobService;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Infrastructure.Services.ScheduledJobService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.ConfigurationSettings;

namespace Infrastructure.ConfigurationService;

public static class ConfigureHangFires
{
    public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
    {
        var hangFireConfig = configuration.GetSection(nameof(HangFireSettings)).Get<HangFireSettings>() ??
                             throw new Exception("HangFireSettings is not configured properly");
        services.Configure<HangFireSettings>(configuration.GetSection(nameof(HangFireSettings)));
        services.ConfigureHangFireDb(hangFireConfig);
        services.AddHangfireServer(serverOptions => { serverOptions.ServerName = hangFireConfig.ServerName; });
        services.AddTransient<IScheduledJobService, HangfireScheduledJobService>();
    }

    public static void ConfigureHangFireDashboard(this WebApplication app, IConfiguration configuration)
    {
        var hangFireConfig = configuration.GetSection(nameof(HangFireSettings)).Get<HangFireSettings>() ??
                             throw new Exception("HangFireSettings is not configured properly");
        app.UseHangfireDashboard(hangFireConfig.Route, new DashboardOptions
        {
            // Authorization = new[] { new HangFireAuthorizationFilter() }
            DashboardTitle = hangFireConfig.DashBoard.DashboardTitle,
            StatsPollingInterval = hangFireConfig.DashBoard.StatsPollingInterval,
            AppPath = hangFireConfig.DashBoard.AppPath,
            IgnoreAntiforgeryToken = true
        });
    }


    #region Private Methods

    private static void ConfigureHangFireDb(this IServiceCollection services, HangFireSettings hangFireSettings)
    {
        switch (hangFireSettings.DatabaseSettings.DbProvider.ToLower())
        {
            case "mongodb":
                ConfigureHangFireMongoDb(services, hangFireSettings);
                break;
            case "sqlserver":
                break;
            case "postgresql":
                break;
            case "mysql":
                break;
            case "redis":
                break;
            default:
                throw new Exception("HangFire database provider is not configured properly or not supported");
        }
    }

    private static void ConfigureHangFireMongoDb(IServiceCollection services, HangFireSettings hangFireSettings)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(hangFireSettings.DatabaseSettings.DefaultConnection);
        var mongoClientSettings =
            MongoClientSettings.FromUrl(new MongoUrl(hangFireSettings.DatabaseSettings.DefaultConnection));

        mongoClientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
        };

        var mongoClient = new MongoClient(mongoClientSettings);

        var mongoStorageOptions = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            Prefix = "SchedulerQueue",
            CheckConnection = false,
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        };

        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseRecommendedSerializerSettings()
                .UseConsole()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            config.UseSerializerSettings(jsonSettings);
        });

        services.AddHangfireConsoleExtensions();
    }

    #endregion
}