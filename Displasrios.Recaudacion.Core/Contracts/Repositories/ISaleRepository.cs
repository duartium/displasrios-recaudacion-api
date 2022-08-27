using Displasrios.Recaudacion.Core.DTOs;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISaleRepository
    {
        int Create(FullOrderDto order);
        IEnumerable<string> GetIncomePerSellers();
    }
}
