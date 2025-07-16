using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Inventory.Product.Domain.GenericRepository;
using MongoDB.Driver;
using Shared.Utils;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetStockByProductId;

public class GetStockByProductIdQueryHandler : IQueryHandler<GetStockByProductIdQuery, GetStockResponse>
{
    private readonly IInventoryEntryRepository _inventoryEntryRepository;

    public GetStockByProductIdQueryHandler(IInventoryEntryRepository inventoryEntryRepository)
    {
        _inventoryEntryRepository = inventoryEntryRepository;
    }

    public async Task<Result<GetStockResponse>> Handle(GetStockByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        var stock = await _inventoryEntryRepository.FindAll()
            .Aggregate().Match(x => x.ProductId == request.ProductId.ToString())
            .Group(x => x.ProductId, g => new { TotalQuantity = g.Sum(x => x.Quantity) })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (stock == null)
        {
            return Result.Success(new GetStockResponse
            {
                Quantity = 0
            });
        }

        return Result.Success(new GetStockResponse
        {
            Quantity = stock.TotalQuantity
        });
    }
}