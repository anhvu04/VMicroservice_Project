using Contracts.Services.MessageBusService;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using Microsoft.Extensions.Logging;
using ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;
using Shared.InfrastructureGrpcModels.CartNotification;

namespace ScheduledJob.Application.Common.HangfireJob;

public class SendCartNotificationScheduleJob
{
    private readonly ILogger<SendCartNotificationScheduleJob> _logger;
    private readonly IMessageBusService _messageBusService;
    private const string CartNotificationRoutingKey = "cart-notification-schedule-routing-key";

    public SendCartNotificationScheduleJob(ILogger<SendCartNotificationScheduleJob> logger,
        IMessageBusService messageBusService)
    {
        _logger = logger;
        _messageBusService = messageBusService;
    }

    public void SendCartNotificationScheduleEvent(SendCartNotificationScheduleGrpcBaseRequest request)
    {
        var cartNotificationEvent = new CartNotificationScheduleEvent
        {
            UserId = request.UserId,
            Items = request.Items.Select(x => new CartItemNotificationScheduleEvent
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList(),
            LastModifiedDate = request.LastModifiedDate
        };
        _messageBusService.PublishMessageAsync(cartNotificationEvent, CartNotificationRoutingKey);
        _logger.LogInformation("Send cart notification event for user: " + cartNotificationEvent.UserId);
    }
}