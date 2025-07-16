using System.Linq.Expressions;
using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Inventory.Product.Domain.GenericRepository;
using MapsterMapper;
using MongoDB.Driver;
using Shared.Utils;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetListInventoryEntry;

public class
    GetListInventoryEntryQueryHandler : IQueryHandler<GetListInventoryEntryQuery,
    PaginationResult<GetInventoryEntryResponse>>
{
    private readonly IInventoryEntryRepository _inventoryEntryRepository;
    private readonly IMapper _mapper;

    public GetListInventoryEntryQueryHandler(IInventoryEntryRepository inventoryEntryRepository, IMapper mapper)
    {
        _inventoryEntryRepository = inventoryEntryRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResult<GetInventoryEntryResponse>>> Handle(GetListInventoryEntryQuery request,
        CancellationToken cancellationToken)
    {
        var inventoryEntries = _inventoryEntryRepository.FindAll();
        var filterBuilder = Builders<Domain.Entities.InventoryEntry>.Filter;
        var filter = filterBuilder.Empty;

        // Apply ProductId filter if provided
        if (request.ProductId != Guid.Empty)
        {
            filter = filterBuilder.Eq(x => x.ProductId, request.ProductId.ToString());
        }

        var query = inventoryEntries.Find(filter);

        // Apply sorting
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            var sortBuilder = Builders<Domain.Entities.InventoryEntry>.Sort;
            var sort = request.IsDescending
                ? sortBuilder.Descending(GetSortProperty(request.OrderBy))
                : sortBuilder.Ascending(GetSortProperty(request.OrderBy));
            query = query.Sort(sort);
        }

        var result = await query.ToPaginatedListAsync(request, _mapper.Map<GetInventoryEntryResponse>);
        return result;
    }

    #region Private Methods

    private Expression<Func<Domain.Entities.InventoryEntry, object>> GetSortProperty(string sortBy)
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