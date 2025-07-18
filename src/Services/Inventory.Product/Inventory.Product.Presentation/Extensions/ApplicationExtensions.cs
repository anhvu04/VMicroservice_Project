using Infrastructure.Extensions.MongoDbExtensions;
using Infrastructure.Middlewares;
using Inventory.Product.Presentation.Grpc.Servers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace Inventory.Product.Presentation.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
        app.MapGrpcService<InventoryEntryGrpcServer>();
        app.MigrateDatabase();
    }

    private static void MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var mongoClient = services.GetRequiredService<IMongoClient>();
        var logger = services.GetRequiredService<ILogger<IMongoClient>>();
        var databaseSettings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
        try
        {
            // Extract database name from connection string
            string databaseName = databaseSettings.DefaultConnection.GetCollectionName();
            var database = mongoClient.GetDatabase(databaseName);
            Migrate(database);

            logger.LogInformation("MongoDB setup completed for database: {DatabaseName}", databaseName);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while setting up MongoDB database");
            throw;
        }
    }

    private static void Migrate(IMongoDatabase database)
    {
        // List of collections to create
        string[] requiredCollections =
        [
            "inventory_entries",
        ];

        // Get existing collections
        var existingCollections = database.ListCollectionNames().ToList();

        // Find collections that need to be created
        var collectionsToCreate = requiredCollections.Except(existingCollections);

        // Create missing collections
        foreach (var collectionName in collectionsToCreate)
        {
            database.CreateCollection(collectionName);
            Console.WriteLine($"Collection '{collectionName}' created.");
        }
    }
}