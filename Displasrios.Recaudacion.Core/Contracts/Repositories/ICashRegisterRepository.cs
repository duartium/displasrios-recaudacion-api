using Displasrios.Recaudacion.Core.DTOs;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ICashRegisterRepository
    {
        bool IsOpenendCash();
        bool Open(decimal initialValue, int idUsuario);
        bool Close(decimal value, string observations, int idUsuario);
        TotalCashCloseDto GetTotalsForCashClose();
    }
}
