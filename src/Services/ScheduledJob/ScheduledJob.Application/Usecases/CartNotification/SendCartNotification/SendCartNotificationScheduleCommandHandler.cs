using Contracts.Common.Interfaces.MediatR;
using Contracts.Services.MessageBusService;
using Contracts.Services.ScheduledJobService;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using Microsoft.Extensions.Logging;
using ScheduledJob.Application.Common.HangfireJob;
using ScheduledJob.Application.Usecases.CartNotification.Common;
using Shared.Utils;

namespace ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;

public class
    SendCartNotificationScheduleCommandHandler : ICommandHandler<SendCartNotificationScheduleCommand,
    GetCartNotificationScheduleResponse>
{
    private readonly IScheduledJobService _scheduledJobService;
    private readonly SendCartNotificationScheduleJob _sendCartNotificationScheduleJob;

    public SendCartNotificationScheduleCommandHandler(IScheduledJobService scheduledJobService,
        SendCartNotificationScheduleJob sendCartNotificationScheduleJob)
    {
        _scheduledJobService = scheduledJobService;
        _sendCartNotificationScheduleJob = sendCartNotificationScheduleJob;
    }

    public Task<Result<GetCartNotificationScheduleResponse>> Handle(SendCartNotificationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var scheduleJob =
            _scheduledJobService.Schedule(
                () => _sendCartNotificationScheduleJob.SendCartNotificationScheduleEvent(request),
                TimeSpan.FromSeconds(30));
        return Task.FromResult(Result.Success(new GetCartNotificationScheduleResponse(scheduleJob)));
    }
}