using Infrastructure.Extensions.MappingExtensions;
using Mapster;
using Product.Repositories.Entities;
using Product.Services.Models.Requests.CatalogProduct;

namespace Product.Services.Mapping;

public static class MappingConfig
{
    public static void RegisterMapping()
    {
        TypeAdapterConfig<UpdateCatalogProductRequest, CatalogProduct>.NewConfig()
            .IgnoreNullProperties();
    }
}