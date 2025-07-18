using MongoDB.Driver;

namespace Infrastructure.Extensions.MongoDbExtensions;

public static class MongoDbExtensions
{
    public static string GetCollectionName(this string connectionString)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(connectionString);
        return mongoUrlBuilder.DatabaseName;
    }
}