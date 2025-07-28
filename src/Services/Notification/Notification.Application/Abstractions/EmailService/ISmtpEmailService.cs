using Notification.Application.Common;

namespace Notification.Application.Abstractions.EmailService;

public interface ISmtpEmailService : IEmailService<SmtpEmailRequest>
{
}