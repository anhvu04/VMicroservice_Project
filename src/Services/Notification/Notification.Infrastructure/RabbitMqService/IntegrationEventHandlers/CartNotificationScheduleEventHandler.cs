using EventBus.Messages.IntegrationEvent.Event;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Usecases.CartNotification.Command;
using Shared.InfrastructureGrpcModels.CartNotification;

namespace Notification.Infrastructure.RabbitMqService.IntegrationEventHandlers;

public class CartNotificationScheduleEventHandler : IConsumer<CartNotificationScheduleEvent>
{
    private readonly ILogger<CartNotificationScheduleEventHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CartNotificationScheduleEventHandler(ILogger<CartNotificationScheduleEventHandler> logger, IMapper mapper,
        ISender sender)
    {
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<CartNotificationScheduleEvent> context)
    {
        _logger.LogInformation("Received cart notification schedule event from cart service");
        var command = _mapper.Map<SendCartNotificationCommand>(context.Message);
        await _sender.Send(command);
    }
}