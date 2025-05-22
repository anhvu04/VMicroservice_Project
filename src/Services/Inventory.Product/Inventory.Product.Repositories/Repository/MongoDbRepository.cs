using Inventory.Product.Repositories.Abstraction;
using Inventory.Product.Repositories.Attributes;
using Inventory.Product.Repositories.Entities.Abstraction;
using Inventory.Product.Repositories.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Inventory.Product.Repositories.Repository;

public class MongoDbRepository<T> : IMongoDbRepository<T> where T : MongoEntity
{
    private readonly IMongoDatabase _database;

    public MongoDbRepository(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        => _database.WithReadPreference(readPreference ?? ReadPreference.Primary).GetCollection<T>(GetCollectionName());

    protected virtual IMongoCollection<T> GetCollection()
        => _database.GetCollection<T>(GetCollectionName());

    public void Create(T entity) => GetCollection().InsertOne(entity);


    public void Update(T entity) => GetCollection().ReplaceOne(x => x.Id == entity.Id, entity);


    public void Delete(string id) => GetCollection().DeleteOne(x => x.Id == id);


    private static string GetCollectionName()
    {
        return typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault() is BsonCollectionAttribute attribute
            ? attribute.CollectionName
            : typeof(T).Name;
    }
}