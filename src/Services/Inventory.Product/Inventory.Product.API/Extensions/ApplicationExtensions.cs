using Inventory.Product.API.GrpcServerServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace Inventory.Product.API.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
        app.MapGrpcService<GetStockGrpcServerService>();
        app.MigrateDatabase();
    }

    private static void MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var mongoClient = services.GetRequiredService<IMongoClient>();
        var logger = services.GetRequiredService<ILogger<IMongoClient>>();
        var mongoClientSettings = services.GetRequiredService<IOptions<MongoDbConnection>>().Value;
        try
        {
            logger.LogInformation("Setting up MongoDB database: {DatabaseName}",
                mongoClientSettings.DatabaseName);

            var database = mongoClient.GetDatabase(mongoClientSettings.DatabaseName);
            Migrate(database);

            logger.LogInformation("MongoDB setup completed for database: {DatabaseName}",
                mongoClientSettings.DatabaseName);
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