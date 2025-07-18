using System.Linq.Expressions;
using Contracts.Services.ScheduledJobService;
using Hangfire;

namespace Infrastructure.Services.HangFireScheduledJobService;

public class HangfireScheduledJobService : IScheduledJobService
{
    public string Enqueue(Expression<Action> functionCall) => BackgroundJob.Enqueue(functionCall);

    public string Enqueue<T>(Expression<Action<T>> functionCall) => BackgroundJob.Enqueue(functionCall);

    public string Schedule(Expression<Action> functionCall, TimeSpan delay) =>
        BackgroundJob.Schedule(functionCall, delay);

    public string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay) =>
        BackgroundJob.Schedule(functionCall, delay);

    public string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt) =>
        BackgroundJob.Schedule(functionCall, enqueueAt);

    public string Schedule<T>(Expression<Action<T>> functionCall, DateTimeOffset enqueueAt) =>
        BackgroundJob.Schedule(functionCall, enqueueAt);

    public string ContinueQueueWith(string parentJobId, Expression<Action> functionCall) =>
        BackgroundJob.ContinueJobWith(parentJobId, functionCall);

    public string ContinueQueueWith(string parentJobId, Expression<Action> functionCall, int continuationOptions)
    {
        return continuationOptions switch
        {
            0 => BackgroundJob.ContinueJobWith(parentJobId, functionCall, JobContinuationOptions.OnAnyFinishedState),
            1 => BackgroundJob.ContinueJobWith(parentJobId, functionCall, JobContinuationOptions.OnlyOnSucceededState),
            2 => BackgroundJob.ContinueJobWith(parentJobId, functionCall, JobContinuationOptions.OnlyOnDeletedState),
            _ => BackgroundJob.ContinueJobWith(parentJobId, functionCall, JobContinuationOptions.OnlyOnSucceededState)
        };
    }

    public bool Delete(string jobId) => BackgroundJob.Delete(jobId);

    public bool Requeue(string jobId) => BackgroundJob.Requeue(jobId);
}