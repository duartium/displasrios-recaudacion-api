using Displasrios.Recaudacion.Core.DTOs;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISaleRepository
    {
        int Create(FullOrderDto order);
    }
}
