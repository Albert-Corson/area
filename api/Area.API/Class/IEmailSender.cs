using System.Threading.Tasks;
using Area.API.Models.Request.Password;
using SendGrid;

namespace Area.API.Class
{
    public interface IEmailSender
    {
        Task SendResetPasswordEmailAsync(string email, string subject, ResetPasswordMailDataModel mailData);
    }
}