using Contracts.Common.Interfaces.MediatR;
using Product.Application.Usecases.CatalogProduct.Common;

namespace Product.Application.Usecases.CatalogProduct.Query.GetCatalogProductById;

public class GetCatalogProductByIdQuery(Guid id) : IQuery<GetCatalogProductResponse>
{
    public Guid Id { get; set; } = id;
}