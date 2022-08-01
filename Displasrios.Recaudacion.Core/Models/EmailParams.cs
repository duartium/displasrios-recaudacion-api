namespace Displasrios.Recaudacion.Core.Models
{
    public class EmailParams
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string EmailTo { get; set; }
        public string Body { get; set; }
    }
}
