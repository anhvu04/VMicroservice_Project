using Contracts.Domains.Interfaces;

namespace Contracts.Domains.EventsEntity;

public class DateTrackingEventEntity<T> : EventEntity<T>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}