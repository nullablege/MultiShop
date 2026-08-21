using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MultiShop.Identity.Configuration;

namespace MultiShop.Identity.Services;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpEmailOptions _options;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(
        IOptions<SmtpEmailOptions> options,
        ILogger<SmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentException.ThrowIfNullOrWhiteSpace(htmlMessage);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

        using var client = new SmtpClient
        {
            Timeout = _options.TimeoutSeconds * 1000
        };

        try
        {
            var socketOptions = _options.UseStartTls
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None;

            await client.ConnectAsync(
                _options.Host,
                _options.Port,
                socketOptions,
                CancellationToken.None);

            if (_options.UseAuthentication)
            {
                await client.AuthenticateAsync(
                    _options.UserName,
                    _options.Password,
                    CancellationToken.None);
            }

            await client.SendAsync(message, CancellationToken.None);
            await client.DisconnectAsync(true, CancellationToken.None);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            _logger.LogError(exception, "Kimlik e-postası SMTP üzerinden gönderilemedi.");
            throw new InvalidOperationException("E-posta gönderilemedi. SMTP yapılandırmasını kontrol edin.", exception);
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true, CancellationToken.None);
            }
        }
    }
}
