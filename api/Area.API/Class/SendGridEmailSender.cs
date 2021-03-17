using System.Threading.Tasks;
using Area.API.Models.Request.Password;
using Area.API.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Area.API.Class
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(
            IOptions<SendGridEmailSenderOptions> options
        )
        {
            this.Options = options.Value;
        }

        private SendGridEmailSenderOptions Options { get; set; }

        public async Task SendResetPasswordEmailAsync(string email, string subject, ResetPasswordMailDataModel mailData)
        {
            await Execute(Options.ApiKey, subject, email, mailData);
        }

        private async Task<Response> Execute(string apiKey, string subject, string email, ResetPasswordMailDataModel mailData)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SenderEmail, Options.SenderName),
                Subject = subject,
            };
            msg.SetTemplateId(Options.Templates.ResetPassword);
            msg.SetTemplateData(mailData);

            msg.AddTo(new EmailAddress(email));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg);
        }
    }
}