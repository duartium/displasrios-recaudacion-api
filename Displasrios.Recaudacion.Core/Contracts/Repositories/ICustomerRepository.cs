using Displasrios.Recaudacion.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto Get(int id);
        CustomerSearchOrderDto GetByIdentification(string identification);
        CustomerSearchOrderDto[] GetByNames(string names);

    }
}
