using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Product.Application.Usecases.CatalogProduct.Common;
using Product.Domain.UnitOfWork;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Query.GetCatalogProductById;

public class GetCatalogProductByIdQueryHandler : IQueryHandler<GetCatalogProductByIdQuery, GetCatalogProductResponse>
{
    private readonly IProductUnitOfWork _productUnitOfWork;
    private readonly IMapper _mapper;

    public GetCatalogProductByIdQueryHandler(IProductUnitOfWork productUnitOfWork, IMapper mapper)
    {
        _productUnitOfWork = productUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCatalogProductResponse>> Handle(GetCatalogProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =
            await _productUnitOfWork.CatalogProduct.FindByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure<GetCatalogProductResponse>(
                $"{nameof(CatalogProduct)} with id: {request.Id} does not exist");
        }

        return Result.Success(_mapper.Map<GetCatalogProductResponse>(entity));
    }
}