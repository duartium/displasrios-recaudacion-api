using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Models.Sales;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISaleRepository
    {
        int Create(FullOrderDto order);
        IEnumerable<IncomeBySellersDto> GetIncomePerSellers(IncomeBySellers incomeBySellers);
    }
}
