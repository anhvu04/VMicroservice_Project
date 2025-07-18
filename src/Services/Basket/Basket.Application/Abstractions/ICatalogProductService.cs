using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;

namespace Basket.Application.Abstractions;

public interface ICatalogProductService
{
    Task<List<GetListCatalogProductsByIdResponse>> GetListCatalogProductsByIdAsync(GetListCatalogProductsByIdRequest byIdRequest);
}