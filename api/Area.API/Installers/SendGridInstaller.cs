using Area.API.Class;
using Area.API.Constants;
using Area.API.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Area.API.Installers
{
    public static class SendGridInstaller
    {
        public static IServiceCollection AddAreaSendGrid(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.ApiKey = configuration[AuthConstants.SendGrid.Key];
                options.SenderEmail = configuration[AuthConstants.SendGrid.SenderMail];
                options.SenderName = configuration[AuthConstants.SendGrid.SenderName];
                options.Templates = new SendGridEmailTemplate()
                {
                    ResetPassword = configuration[AuthConstants.SendGrid.TemplateResetPassword]
                };
            });

            return services;
        }
    }
}