using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.InfrastructureServiceModels.EmailServiceModel;

public class SmtpEmailRequest
{
    public ToEmail ToEmail { get; set; } = null!;
    public List<ToEmail> ListToEmail { get; set; } = [];
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public IFormFileCollection Attachments { get; set; } = null!;
}

public  class ToEmail
{
    [EmailAddress] public string To { get; set; } = null!;
}