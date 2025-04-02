using Infrastructure.Mapping;
using Mapster;
using Product.Repositories.Entities;
using Product.Services.Models.Request.CatalogProduct;

namespace Product.Services.Mapping;

public static class MappingConfig
{
    public static void RegisterMapping()
    {
        TypeAdapterConfig<UpdateCatalogProductRequest, CatalogProduct>.NewConfig()
            .IgnoreNullProperties();
    }
}