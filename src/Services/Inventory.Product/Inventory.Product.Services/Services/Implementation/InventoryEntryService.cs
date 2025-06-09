using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Inventory.Product.Repositories.Entities;
using Inventory.Product.Services.Models.Requests.InventoryEntry;
using Inventory.Product.Services.Models.Responses.InventoryEntry;
using Inventory.Product.Services.Services.Interfaces;
using Inventory.Product.Services.Shared;
using Inventory.Product.Services.Shared.Pagination;
using MapsterMapper;
using MongoDB.Driver;

namespace Inventory.Product.Services.Services.Implementation;

public class InventoryEntryService : IInventoryEntryService
{
    private readonly IMongoDbRepository<InventoryEntry> _inventoryEntryRepository;
    private readonly IMapper _mapper;

    public InventoryEntryService(IMongoDbRepository<InventoryEntry> inventoryEntryRepository, IMapper mapper)
    {
        _inventoryEntryRepository = inventoryEntryRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResult<InventoryEntryResponse>>> GetInventoryEntries(
        GetInventoryEntryRequest request)
    {
        var inventoryEntries = _inventoryEntryRepository.FindAll();
        var filterBuilder = Builders<InventoryEntry>.Filter;
        var filter = filterBuilder.Empty;

        // Apply ProductId filter if provided
        if (request.ProductId != Guid.Empty)
        {
            filter = filterBuilder.Eq(x => x.ProductId, request.ProductId.ToString());
        }

        var query = inventoryEntries.Find(filter);

        var sortBuilder = Builders<InventoryEntry>.Sort;
        var sort = request.IsDescending
            ? sortBuilder.Descending(GetSortProperty(request.OrderBy))
            : sortBuilder.Ascending(GetSortProperty(request.OrderBy));
        query = query.Sort(sort);

        var result = await query.ToPaginatedListAsync(request, _mapper.Map<InventoryEntryResponse>);
        return result;
    }

    public async Task<Result<InventoryEntryResponse>> GetInventoryEntryById(string id)
    {
        // Check if the id is a valid ObjectId
        if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
        {
            return Result.Failure<InventoryEntryResponse>("Invalid inventory entry ID format");
        }

        var inventoryEntry = await _inventoryEntryRepository.FindAll()
            .Find(x => x.Id == id)
            .Project(x => new InventoryEntryResponse
            {
                Id = x.Id,
                DocumentNo = x.DocumentNo,
                DocumentType = x.DocumentType.ToString(),
                ProductId = x.ProductId.ToString(),
                Quantity = x.Quantity,
                ExternalDocumentNo = x.ExternalDocumentNo
            }).FirstOrDefaultAsync();

        if (inventoryEntry == null)
        {
            return Result.Failure<InventoryEntryResponse>("Inventory entry not found");
        }

        return inventoryEntry;
    }

    public Task<Result> PurchaseProduct(PurchaseProductRequest request)
    {
        var purchaseProduct = new InventoryEntry()
        {
            DocumentNo = Guid.NewGuid().ToString(),
            ProductId = request.ProductId.ToString(),
            Quantity = request.Quantity,
            DocumentType = DocumentType.Purchase,
            ExternalDocumentNo = Guid.NewGuid().ToString()
        };
        _inventoryEntryRepository.Create(purchaseProduct);
        return Task.FromResult(Result.Success());
    }

    public async Task<Result<int>> GetStock(string productId)
    {
        var stock = await _inventoryEntryRepository.FindAll()
            .Aggregate().Match(x => x.ProductId == productId)
            .Group(x => x.ProductId, g => new { TotalQuantity = g.Sum(x => x.Quantity) })
            .FirstOrDefaultAsync();
        return stock?.TotalQuantity ?? 0;
    }

    #region Private Methods

    private Expression<Func<InventoryEntry, object>> GetSortProperty(string sortBy)
    {
        return sortBy switch
        {
            "productid" => x => x.ProductId,
            "quantity" => x => x.Quantity,
            _ => x => x.CreatedDate
        };
    }

    #endregion
}