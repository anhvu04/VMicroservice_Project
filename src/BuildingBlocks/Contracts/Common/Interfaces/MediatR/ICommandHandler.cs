using MediatR;
using Shared.Utils;

namespace Contracts.Common.Interfaces.MediatR;

public interface ICommandHandler<TCommand> : IRequestHandler<ICommand, Result> where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}