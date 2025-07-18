using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Product.Application.Usecases.CatalogProduct.Common;
using Product.Domain.UnitOfWork;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Command.CreateCatalogProduct;

public class
    CreateCatalogProductCommandHandler : ICommandHandler<CreateCatalogProductCommand, CreateCatalogProductResponse>
{
    private readonly IProductUnitOfWork _productUnitOfWork;
    private readonly IMapper _mapper;

    public CreateCatalogProductCommandHandler(IProductUnitOfWork productUnitOfWork, IMapper mapper)
    {
        _productUnitOfWork = productUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateCatalogProductResponse>> Handle(CreateCatalogProductCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.CatalogProduct>(request);
        _productUnitOfWork.CatalogProduct.Add(entity);
        await _productUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new CreateCatalogProductResponse()
        {
            Id = entity.Id
        });
    }
}