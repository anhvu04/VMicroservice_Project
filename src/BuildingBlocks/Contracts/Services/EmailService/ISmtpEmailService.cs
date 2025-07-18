using Shared.InfrastructureServiceModels.EmailServiceModel;

namespace Contracts.Services.EmailService;

public interface ISmtpEmailService : IEmailService<SmtpEmailRequest>
{
}