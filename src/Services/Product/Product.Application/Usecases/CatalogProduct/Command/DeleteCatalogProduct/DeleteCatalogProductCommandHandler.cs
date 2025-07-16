using Contracts.Common.Interfaces.MediatR;
using Product.Domain.UnitOfWork;
using Shared.Utils;

namespace Product.Application.Usecases.CatalogProduct.Command.DeleteCatalogProduct;

public class DeleteCatalogProductCommandHandler : ICommandHandler<DeleteCatalogProductCommand>
{
    private readonly IProductUnitOfWork _productUnitOfWork;

    public DeleteCatalogProductCommandHandler(IProductUnitOfWork productUnitOfWork)
    {
        _productUnitOfWork = productUnitOfWork;
    }

    public async Task<Result> Handle(DeleteCatalogProductCommand request, CancellationToken cancellationToken)
    {
        var entity =
            await _productUnitOfWork.CatalogProduct.FindByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return Result.Failure($"{nameof(CatalogProduct)} with id: {request.Id} does not exist");
        }

        _productUnitOfWork.CatalogProduct.Delete(entity);
        await _productUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}