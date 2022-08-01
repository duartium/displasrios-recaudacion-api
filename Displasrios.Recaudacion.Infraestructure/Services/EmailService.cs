using Displasrios.Recaudacion.Core.Contracts.Services;
using Displasrios.Recaudacion.Core.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Services
{
    public class EmailService : IEmailService
    {
        public void Send(EmailParams emailParams, out string message)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(emailParams.SenderEmail, emailParams.SenderName);
            msg.Subject = emailParams.Subject;
            msg.To.Add(emailParams.EmailTo);
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = emailParams.Body;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.displasrios.com";
            smtp.Port = 8889;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("asistencia@displasrios.com", "4*Smov6Rg28p");

            string output = null;
            try
            {
                smtp.Send(msg);
                msg.Dispose();
                output = "Correo electrónico fue enviado satisfactoriamente.";
                message = output;
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
                message = output;
            }
        }

    }
}
