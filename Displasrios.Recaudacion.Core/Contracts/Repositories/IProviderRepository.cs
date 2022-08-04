using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IProviderRepository
    {
        IEnumerable<ProviderDto> GetAll();
        ProviderDto GetById(int id);
        ProviderDto GetByName(string name);
        IEnumerable<ItemCatalogueDto> GetAsCatalogue();
        bool Create(ProviderCreate provider);
        bool Remove(int id);
    }
}
