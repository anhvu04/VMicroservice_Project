using Shared.Utils;

namespace Contracts.Services.EmailService;

public interface IEmailService<in T>
{
    Task<Result> SendMailAsync(T request, CancellationToken cancellationToken);
}