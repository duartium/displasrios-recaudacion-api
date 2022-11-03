using Displasrios.Recaudacion.Core.DTOs;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISellerRepository
    {
        CustomerDebtDto GetDebts(int idSeller);
    }
}