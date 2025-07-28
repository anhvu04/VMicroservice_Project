using Shared.Utils;

namespace Notification.Application.Abstractions.EmailService;

public interface IEmailService<in T>
{
    Task<Result> SendMailAsync(T request, CancellationToken cancellationToken);
}