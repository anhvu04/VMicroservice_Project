using MediatR;
using Shared.Utils;

namespace Contracts.Common.Interfaces.MediatR;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}