using ChatApp.Server.Application.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace ChatApp.Server.Infrastructure.Services;
internal sealed class EmailService : IEmailService
{
    private readonly IConfiguration configuration;
    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = configuration["EmailSettings:Email"];
        var password = configuration["EmailSettings:Password"];
        var host = configuration["EmailSettings:Host"];
        var port = configuration.GetValue<int>("EmailSettings:Port");

        var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(email, password),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(email!, "Romaeterna Chat"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(to);
        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}
