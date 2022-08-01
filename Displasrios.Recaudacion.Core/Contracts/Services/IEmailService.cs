using Displasrios.Recaudacion.Core.Models;

namespace Displasrios.Recaudacion.Core.Contracts.Services
{
    public interface IEmailService
    {
        void Send(EmailParams emailParams, out string message);
    }
}
