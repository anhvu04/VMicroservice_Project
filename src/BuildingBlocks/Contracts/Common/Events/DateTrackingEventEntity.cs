using Contracts.Domains.Interfaces;

namespace Contracts.Common.Events;

public class DateTrackingEventEntity<T> : EventEntity<T>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}