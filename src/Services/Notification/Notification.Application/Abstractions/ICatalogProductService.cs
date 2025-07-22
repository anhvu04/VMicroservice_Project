using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

namespace Notification.Application.Abstractions;

public interface ICatalogProductService
{
    Task<List<GetListCatalogProductsByIdGrpcBaseResponse>> GetListCatalogProductsByIdAsync(
        GetListCatalogProductsByIdGrpcBaseRequest byIdGrpcBaseRequest);
}