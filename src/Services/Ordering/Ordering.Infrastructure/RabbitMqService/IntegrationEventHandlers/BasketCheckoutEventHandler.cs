using EventBus.Messages.IntegrationEvent.Event;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Usecases.Order.Command.CreateOrder;

namespace Ordering.Infrastructure.RabbitMqService.IntegrationEventHandlers;

public class BasketCheckoutEventHandler : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutEventHandler> _logger;
    private readonly IMapper _mapper;

    public BasketCheckoutEventHandler(IMediator mediator, ILogger<BasketCheckoutEventHandler> logger, IMapper mapper)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        _logger.LogInformation($"BasketCheckoutEventHandler consumed: {context.Message}");
        var req = _mapper.Map<CreateOrderCommand>(context.Message);
        await _mediator.Send(req);
    }
}