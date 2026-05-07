using Exercices.Courriels.Web.Configuration;
using Exercices.Courriels.Web.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Exercices.Courriels.Web.Services
{
    public class EmailService(EmailConfiguration options, IWebHostEnvironment env) : IEmailSender
    {
        private readonly EmailConfiguration _options = options;
        private readonly IWebHostEnvironment _env = env;

        public async Task Send(string to, string subject, string html, string? from = null)
        {
            await SendAsync(to, subject, html, from);
        }

        private async Task SendAsync(string to, string subject, string html, string? from = null)
        {
            // Utiliser un fournisseur d'envoi de courriel. Autrement, changer pour utiliser un serveur SMTP
            await SendWithMailtrapAsync(to, subject, html, from);
        }
        private async Task SendWithMailtrapAsync(string to, string subject, string html, string? from = null)
        {
            using var client = new SmtpClient(_options.SmtpServer, _options.Port)
            {
                Credentials = new NetworkCredential(_options.UserName, _options.Password),
                EnableSsl = true
            };
            client.Send(from ?? _options.From, to, subject, html);

        }

        private async Task SendWithSmtpAsync(string to, string subject, string html, string? from = null)
        {
            // Fallback SMTP pour compatibilité (ex: Ethereal.email en dev)
            var email = new MimeKit.MimeMessage();
            email.From.Add(MimeKit.MailboxAddress.Parse(from ?? _options.From));
            email.To.Add(MimeKit.MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_options.SmtpServer, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_options.UserName, _options.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public void SendWithTemplate(string to, string subject, string? from = null)
        {
            var pathToFile = Path.Join(_env.WebRootPath, "Templates", "Emails", "SubscriptionNew.html");

            var builder = new MimeKit.BodyBuilder();
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            Send(to, subject, builder.HtmlBody, from);
        }
    }
}