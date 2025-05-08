using Shared.Services.EmailService;

namespace Contracts.Services.EmailService;

public interface ISmtpEmailService : IEmailService<SmtpEmailRequest>
{
}