using MediatR;
using Shared.Utils;

namespace Contracts.Common.Interfaces.MediatR;

public interface IQuery : IRequest<Result>
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}