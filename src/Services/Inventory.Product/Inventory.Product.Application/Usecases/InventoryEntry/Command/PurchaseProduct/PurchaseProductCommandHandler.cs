using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Domain.Entities;
using Inventory.Product.Domain.GenericRepository;
using Shared.Utils;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Command.PurchaseProduct;

public class PurchaseProductCommandHandler : ICommandHandler<PurchaseProductCommand>
{
    private readonly IInventoryEntryRepository _inventoryEntryRepository;

    public PurchaseProductCommandHandler(IInventoryEntryRepository inventoryEntryRepository)
    {
        _inventoryEntryRepository = inventoryEntryRepository;
    }

    public Task<Result> Handle(PurchaseProductCommand request, CancellationToken cancellationToken)
    {
        var purchaseProduct = new Domain.Entities.InventoryEntry
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
}