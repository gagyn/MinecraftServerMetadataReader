using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HypixelCounter.Services;

namespace HypixelCounterServer.Service
{
    public class NotificationService
    {
        private readonly AppConfiguration _appConfiguration;

        public NotificationService(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public async Task SendMail(int count)
        {
            var from = new MailAddress(_appConfiguration.FromMailAddress);
            var to = new MailAddress(_appConfiguration.ToMailAddress);
            var mail = new MailMessage(from, to)
            {
                Subject = _appConfiguration.DefaultMessageSubject,
                Body = _appConfiguration.DefaultMessageText.Replace("{{count}}", count.ToString())
            };

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            using var client = new SmtpClient(_appConfiguration.SmtpClientAddress, _appConfiguration.SmtpClientPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                TargetName = "STARTTLS/smtp.gmail.com",
                Credentials = new NetworkCredential(_appConfiguration.SmtpClientLogin, _appConfiguration.SmtpClientPassword)
            };

            await client.SendMailAsync(mail);
        }
    }
}
