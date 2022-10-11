namespace Displasrios.Recaudacion.Core.Models
{
    public class MailSettings
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DisplayName { get; set; }
        public string SendEmailToMe { get; set; }
        public string EmailTo { get; set; }
        public string SendEmail { get; set; }
    }
}