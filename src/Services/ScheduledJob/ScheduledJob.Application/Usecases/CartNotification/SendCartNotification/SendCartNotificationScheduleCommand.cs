using Contracts.Common.Interfaces.MediatR;
using ScheduledJob.Application.Usecases.CartNotification.Common;
using Shared.InfrastructureServiceModels.CartNotification;

namespace ScheduledJob.Application.Usecases.CartNotification.SendCartNotification;

public class SendCartNotificationScheduleCommand : SendCartNotificationScheduleRequest, ICommand<GetCartNotificationScheduleResponse>
{
}