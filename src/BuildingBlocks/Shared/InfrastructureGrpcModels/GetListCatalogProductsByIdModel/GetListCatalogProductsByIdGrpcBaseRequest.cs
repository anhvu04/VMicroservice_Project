using Shared.MediatR;

namespace Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

public class GetListCatalogProductsByIdGrpcBaseRequest : IQuery<List<GetListCatalogProductsByIdGrpcBaseResponse>>
{
    public List<Guid> Ids { get; set; } = [];
}