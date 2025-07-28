using Contracts.Services.ScheduledJobService;
using ScheduledJob.Application.Common.HangfireJob;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.MediatR;
using Shared.Utils;

namespace ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;

public class SendCartNotificationScheduleCommandHandler : ICommandHandler<SendCartNotificationScheduleGrpcBaseRequest,
    SendCartNotificationScheduleGrpcBaseResponse>
{
    private readonly IScheduledJobService _scheduledJobService;
    private readonly SendCartNotificationScheduleJob _sendCartNotificationScheduleJob;

    public SendCartNotificationScheduleCommandHandler(IScheduledJobService scheduledJobService,
        SendCartNotificationScheduleJob sendCartNotificationScheduleJob)
    {
        _scheduledJobService = scheduledJobService;
        _sendCartNotificationScheduleJob = sendCartNotificationScheduleJob;
    }

    public Task<Result<SendCartNotificationScheduleGrpcBaseResponse>> Handle(
        SendCartNotificationScheduleGrpcBaseRequest request, CancellationToken cancellationToken)
    {
        // Delete old job
        if (!string.IsNullOrEmpty(request.JobId))
        {
            _scheduledJobService.Delete(request.JobId);
        }

        var scheduleJobId = string.Empty;
        // Schedule new job if there are any items
        if (request.Items.Count != 0)
        {
            scheduleJobId = _scheduledJobService.Schedule(
                () => _sendCartNotificationScheduleJob.SendCartNotificationScheduleEvent(request),
                TimeSpan.FromSeconds(30));
            return Task.FromResult(Result.Success(new SendCartNotificationScheduleGrpcBaseResponse(scheduleJobId)));
        }

        return Task.FromResult(Result.Success(new SendCartNotificationScheduleGrpcBaseResponse(scheduleJobId)));
    }
}