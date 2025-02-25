using HR.CRUDWebApi.CQRS_MediatR.Sample.Events.Notifications;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Polly.Retry;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Handlers
{
    public class SendWelComeEmailHandler : INotificationHandler<SaveUserDetailsNotification>
    {
        private readonly ILogger<SendWelComeEmailHandler> _logger;
        private readonly MailSettingsDto _mailSettingsDto;
        private readonly SmtpClient _smtpClient;
        private readonly AsyncRetryPolicy _asyncRetryPolicy;
        public SendWelComeEmailHandler(ILogger<SendWelComeEmailHandler> logger, IOptions<MailSettingsDto> mailSettings, SmtpClient smtpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailSettingsDto = mailSettings.Value ?? throw new ArgumentNullException(nameof(mailSettings));
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
            _asyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2 * i),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Retry {retryCount} due to error: {exception.Message}");
                });
        }
        public async Task Handle(SaveUserDetailsNotification notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                _logger.LogError("Invalid notification");
                return;
            }
            try
            {
                var mail = CreateEmailMessage(notification);
                _logger.LogInformation($"Sending email to {notification.Email}..");
                await _asyncRetryPolicy.ExecuteAsync(async () => await SendEmail(mail, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email to {notification.Email}. Error: {ex.Message}");
            }
        }

        private async Task SendEmail(MimeMessage mail, CancellationToken cancellationToken)
        {
            await _smtpClient.ConnectAsync(_mailSettingsDto.Host, _mailSettingsDto.Port, GetSecureSocketOption(), cancellationToken);
            await _smtpClient.AuthenticateAsync(_mailSettingsDto.UserName, _mailSettingsDto.Password, cancellationToken);
            await _smtpClient.SendAsync(mail, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(SaveUserDetailsNotification notification)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_mailSettingsDto.DisPlayName, _mailSettingsDto.From));
            mail.To.Add(new MailboxAddress(notification.FirstName, notification.Email));
            mail.Subject = "Welcome to Love in Air !!";
            mail.Body = new TextPart("plain")
            {
                Text = $"Dear {notification.FirstName},\n\n" +
                $"Welcome to Love in Air. We are excited to have you on board.\n\n" +
                $"Best Regards,\n" +
                $"Love in Air Team"
            };
            return mail;
        }
        private SecureSocketOptions GetSecureSocketOption()
        {
            return _mailSettingsDto.UseSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
        }
    }
}
