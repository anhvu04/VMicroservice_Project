using System.Linq.Expressions;

namespace Contracts.Services.ScheduledJobService;

public interface IScheduledJobService
{
    #region Fire And Forget Jobs

    /// <summary>
    /// Fire and forget. Ex: The job will be executed immediately and no repeat.
    /// </summary>
    /// <param name="functionCall"></param>
    /// <returns></returns>
    string Enqueue(Expression<Action> functionCall);

    /// <summary>
    /// Fire and forget. Ex: The job will be executed immediately with parameter and no repeat.
    /// </summary>
    /// <param name="functionCall"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string Enqueue<T>(Expression<Action<T>> functionCall);

    #endregion

    #region Delayed Jobs

    /// <summary>
    /// Delayed. Ex: After ${delay} minutes, the job will be executed and no repeat.
    /// </summary>
    /// <param name="functionCall"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    string Schedule(Expression<Action> functionCall, TimeSpan delay);

    /// <summary>
    /// Delayed. Ex: After ${delay} minutes, the job will be executed with parameter and no repeat.
    /// </summary>
    /// <param name="functionCall"></param>
    /// <param name="delay"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay);

    /// <summary>
    /// Delayed. Ex: At ${enqueueAt}, the job will be executed and no repeat. (Execute exactly at the time)
    /// </summary>
    /// <param name="functionCall"></param>
    /// <param name="enqueueAt"></param>
    /// <returns></returns>
    string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt);

    /// <summary>
    /// Delayed. Ex: At ${enqueueAt}, the job will be executed with parameter and no repeat. (Execute exactly at the time)
    /// </summary>
    /// <param name="functionCall"></param>
    /// <param name="enqueueAt"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string Schedule<T>(Expression<Action<T>> functionCall, DateTimeOffset enqueueAt);

    #endregion

    #region Continuous Jobs

    /// <summary>
    /// Continuous. Ex: The job will be executed after the parent job is completed.
    /// </summary>
    /// <param name="parentJobId"></param>
    /// <param name="functionCall"></param>
    /// <returns></returns>
    string ContinueQueueWith(string parentJobId, Expression<Action> functionCall);

    /// <summary>
    /// Continuous. Ex: The job will be executed after the parent job is completed with options.
    /// </summary>
    /// <param name="parentJobId"></param>
    /// <param name="functionCall"></param>
    /// <param name="continuationOptions">
    /// 0. OnAnyFinishedState
    /// 1. OnlyOnSucceededState
    /// 2. OnlyOnDeletedState
    /// Default: OnlyOnSucceededState
    /// </param>
    /// <returns></returns>
    string ContinueQueueWith(string parentJobId, Expression<Action> functionCall, int continuationOptions);

    #endregion

    /// <summary>
    /// Remove job from queue
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    bool Delete(string jobId);

    /// <summary>
    /// Requeue job
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    bool Requeue(string jobId);
}