using Infrastructure.Extensions.MappingExtensions;
using Mapster;
using Product.Application.Usecases.CatalogProduct.Command.UpdateCatalogProduct;
using Product.Domain.Entities;

namespace Product.Application.Mapping;

public static class MappingConfig
{
    public static void RegisterMapping()
    {
        TypeAdapterConfig<UpdateCatalogProductCommand, CatalogProduct>.NewConfig().IgnoreNullProperties();
    }
}