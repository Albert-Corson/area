namespace Area.API.Options
{
    public class SendGridEmailTemplate
    {
        public string ResetPassword { get; set; } = null!;
    }
    public class SendGridEmailSenderOptions
    {
        public string ApiKey { get; set; } = null!;

        public string SenderEmail { get; set; } = null!;

        public string SenderName { get; set; } = null!;

        public SendGridEmailTemplate Templates { get; set; } = null!;
    }
}