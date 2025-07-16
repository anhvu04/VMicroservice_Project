using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Inventory.Product.Domain.GenericRepository;
using MongoDB.Driver;
using Shared.Utils;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetInventoryEntryById;

public class GetInventoryEntryByIdQueryHandler : IQueryHandler<GetInventoryEntryByIdQuery, GetInventoryEntryResponse>
{
    private readonly IInventoryEntryRepository _inventoryEntryRepository;

    public GetInventoryEntryByIdQueryHandler(IInventoryEntryRepository inventoryEntryRepository)
    {
        _inventoryEntryRepository = inventoryEntryRepository;
    }

    public async Task<Result<GetInventoryEntryResponse>> Handle(GetInventoryEntryByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if the id is a valid ObjectId
        if (!MongoDB.Bson.ObjectId.TryParse(request.Id, out _))
        {
            return Result.Failure<GetInventoryEntryResponse>("Invalid inventory entry ID format");
        }

        var inventoryEntry = await _inventoryEntryRepository.FindAll()
            .Find(x => x.Id == request.Id)
            .Project(x => new GetInventoryEntryResponse
            {
                Id = x.Id,
                DocumentNo = x.DocumentNo,
                DocumentType = x.DocumentType.ToString(),
                ProductId = x.ProductId.ToString(),
                Quantity = x.Quantity,
                ExternalDocumentNo = x.ExternalDocumentNo ?? string.Empty,
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (inventoryEntry == null)
        {
            return Result.Failure<GetInventoryEntryResponse>("Inventory entry not found");
        }

        return inventoryEntry;
    }
}