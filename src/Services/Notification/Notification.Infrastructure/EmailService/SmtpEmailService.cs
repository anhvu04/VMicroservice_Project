using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Application.Abstractions.EmailService;
using Notification.Application.Common;
using Shared.ConfigurationSettings;
using Shared.Utils;

namespace Notification.Infrastructure.EmailService;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailSettings> emailSettings,
        ILogger<SmtpEmailService> logger)
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }

    public async Task<Result> SendMailAsync(SmtpEmailRequest request, CancellationToken cancellationToken)
    {
        var mailKit = new MimeMessage
        {
            From = { new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail) },
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };

        if (request.ListToEmail.Count != 0)
        {
            request.ListToEmail.ForEach(x => mailKit.To.Add(MailboxAddress.Parse(x.To)));
        }
        else
        {
            mailKit.To.Add(MailboxAddress.Parse(request.ToEmail.To));
        }

        using var smtpClient = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.EnableSsl,
                cancellationToken);
            await smtpClient.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password, cancellationToken);
            await smtpClient.SendAsync(mailKit, cancellationToken);
            _logger.LogInformation("Email sent successfully to {ToEmail}", request.ToEmail.To);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send email: {Message}", ex.Message);
            return Result.Failure($"Failed to send email: {ex.Message}");
        }
        finally
        {
            await smtpClient.DisconnectAsync(true, cancellationToken);
        }

        return Result.Success();
    }
}