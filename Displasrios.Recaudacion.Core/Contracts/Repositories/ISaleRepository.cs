using Displasrios.Recaudacion.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface ISaleRepository
    {
        int Create(FullOrderDto order);
    }
}
