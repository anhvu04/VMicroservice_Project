using Product.Application.Usecases.CatalogProduct.Common;
using Shared.MediatR;

namespace Product.Application.Usecases.CatalogProduct.Query.GetCatalogProductById;

public class GetCatalogProductByIdQuery(Guid id) : IQuery<GetCatalogProductResponse>
{
    public Guid Id { get; set; } = id;
}