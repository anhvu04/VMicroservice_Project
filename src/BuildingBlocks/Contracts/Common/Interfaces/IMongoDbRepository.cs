using Contracts.Domains;
using MongoDB.Driver;

namespace Contracts.Common.Interfaces;

public interface IMongoDbRepository<T> where T : MongoEntity
{
    IMongoCollection<T> FindAll(ReadPreference? readPreference = null);
    void Create(T entity);
    void Update(T entity);
    void Delete(string id);
}