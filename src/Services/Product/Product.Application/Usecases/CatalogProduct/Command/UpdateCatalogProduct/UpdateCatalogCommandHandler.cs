using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Product.Domain.UnitOfWork;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Command.UpdateCatalogProduct;

public class UpdateCatalogCommandHandler : ICommandHandler<UpdateCatalogProductCommand>
{
    private readonly IProductUnitOfWork _productUnitOfWork;
    private readonly IMapper _mapper;

    public UpdateCatalogCommandHandler(IProductUnitOfWork productUnitOfWork, IMapper mapper)
    {
        _productUnitOfWork = productUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateCatalogProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productUnitOfWork.CatalogProduct.FindByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure($"{nameof(CatalogProduct)} with id: {request.Id} does not exist");
        }

        _mapper.Map(request, entity);
        _productUnitOfWork.CatalogProduct.Update(entity);
        await _productUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}