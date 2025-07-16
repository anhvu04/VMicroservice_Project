using Infrastructure.Common.Implementation;
using Inventory.Product.Domain.Entities;
using Inventory.Product.Domain.GenericRepository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace Inventory.Product.Persistence.GenericRepository;

public class InventoryEntryRepository : MongoDbRepository<InventoryEntry>, IInventoryEntryRepository
{
    public InventoryEntryRepository(IMongoClient client, IOptions<DatabaseSettings> settings) : base(client, settings)
    {
    }
}