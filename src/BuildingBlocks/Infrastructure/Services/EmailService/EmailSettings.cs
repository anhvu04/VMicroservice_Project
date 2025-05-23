using Contracts.Services.EmailService;

namespace Infrastructure.Services.EmailService;

public class EmailSettings : IEmailSettings
{
    public string SmtpServer { get; set; } = null!;
    public int Port { get; set; }
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool EnableSsl { get; set; }
}