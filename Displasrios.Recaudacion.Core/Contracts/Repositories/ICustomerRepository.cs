using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto Get(int id);
        CustomerSearchOrderDto GetByIdentification(string identification);
        CustomerSearchOrderDto[] GetByNames(string names);
        bool Update(CustomerUpdate customer);
        int Create(CustomerCreate customer);
        bool Delete(int id);
        CustomerDebtDto GetDebts(string identification, string names);
        IEnumerable<BestCustomerDto> GetBestCustomers();
    }
}
