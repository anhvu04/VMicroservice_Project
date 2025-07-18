using Contracts.Common.Interfaces.MediatR;

namespace Product.Application.Usecases.CatalogProduct.Command.DeleteCatalogProduct;

public class DeleteCatalogProductCommand(Guid id) : ICommand
{
    public Guid Id { get; set; } = id;
}