using Shared.ServiceModels.EmailServiceModel;

namespace Contracts.Services.EmailService;

public interface ISmtpEmailService : IEmailService<SmtpEmailRequest>
{
}