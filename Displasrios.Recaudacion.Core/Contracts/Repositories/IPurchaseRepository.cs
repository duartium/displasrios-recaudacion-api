using Displasrios.Recaudacion.Core.Models;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IPurchaseRepository
    {
        bool Register(PurchaseCreate purchase);
    }
}
