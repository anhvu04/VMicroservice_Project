using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.Utils;

namespace Basket.Application.Abstractions;

public interface ICatalogProductService
{
    Task<Result<List<GetListCatalogProductsByIdGrpcBaseResponse>>> GetListCatalogProductsByIdAsync(GetListCatalogProductsByIdGrpcBaseRequest byIdGrpcBaseRequest);
}