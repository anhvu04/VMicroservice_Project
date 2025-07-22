using MediatR;
using Shared.Utils;

namespace Shared.MediatR;

public interface IQuery : IRequest<Result>
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}