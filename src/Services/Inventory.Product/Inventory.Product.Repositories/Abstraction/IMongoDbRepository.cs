using Inventory.Product.Repositories.Entities.Abstraction;
using MongoDB.Driver;

namespace Inventory.Product.Repositories.Abstraction;

public interface IMongoDbRepository<T> where T : MongoEntity
{
    IMongoCollection<T> FindAll(ReadPreference? readPreference = null);
    void Create(T entity);
    void Update(T entity);
    void Delete(string id);
}