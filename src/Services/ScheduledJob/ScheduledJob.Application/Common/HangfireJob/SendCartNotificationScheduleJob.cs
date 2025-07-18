using Contracts.Services.MessageBusService;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using Microsoft.Extensions.Logging;
using ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;

namespace ScheduledJob.Application.Common.HangfireJob;

public class SendCartNotificationScheduleJob
{
    private readonly ILogger<SendCartNotificationScheduleJob> _logger;
    private readonly IMessageBusService _messageBusService;

    public SendCartNotificationScheduleJob(ILogger<SendCartNotificationScheduleJob> logger,
        IMessageBusService messageBusService)
    {
        _logger = logger;
        _messageBusService = messageBusService;
    }

    public void SendCartNotificationScheduleEvent(SendCartNotificationScheduleCommand command)
    {
        var cartNotificationEvent = new CartNotificationScheduleEvent
        {
            UserId = command.UserId,
            Items = command.Items.Select(x => new CartItemNotificationScheduleEvent
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList(),
            LastModifiedDate = command.LastModifiedDate
        };
        _messageBusService.PublishMessageAsync(cartNotificationEvent, "CartNotificationEvent");
        _logger.LogInformation("Send cart notification event for user: " + cartNotificationEvent.UserId);
    }
}