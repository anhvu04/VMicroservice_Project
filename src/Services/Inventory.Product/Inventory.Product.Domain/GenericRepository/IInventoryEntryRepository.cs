using Contracts.Common.Interfaces;
using Inventory.Product.Domain.Entities;

namespace Inventory.Product.Domain.GenericRepository;

public interface IInventoryEntryRepository : IMongoDbRepository<InventoryEntry>
{
}