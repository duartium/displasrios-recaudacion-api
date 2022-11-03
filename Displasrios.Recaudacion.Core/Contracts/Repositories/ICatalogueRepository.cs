using Displasrios.Recaudacion.Core.DTOs;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ICatalogueRepository
    {
        List<CatalogueDto> GetAll();
        IEnumerable<ItemCatalogueDto> GetSellers();
        CatalogueDto Get(string name);
    }
}
