using Contracts.Domains.EventsEntity;
using MediatR;

namespace Infrastructure.Extensions.MediatorExtentions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}