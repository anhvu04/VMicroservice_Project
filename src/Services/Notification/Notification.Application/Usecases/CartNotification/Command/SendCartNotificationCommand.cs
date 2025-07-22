using Shared.MediatR;

namespace Notification.Application.Usecases.CartNotification.Command;

public class SendCartNotificationCommand : ICommand
{
    public Guid UserId { get; set; }
    public List<SendCartItemNotificationCommand> Items { get; set; } = null!;
    public DateTime LastModifiedDate { get; set; }
}

public class SendCartItemNotificationCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}