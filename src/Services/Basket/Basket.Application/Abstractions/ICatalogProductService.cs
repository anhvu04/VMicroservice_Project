using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

namespace Basket.Application.Abstractions;

public interface ICatalogProductService
{
    Task<List<GetListCatalogProductsByIdGrpcBaseResponse>> GetListCatalogProductsByIdAsync(GetListCatalogProductsByIdGrpcBaseRequest byIdGrpcBaseRequest);
}