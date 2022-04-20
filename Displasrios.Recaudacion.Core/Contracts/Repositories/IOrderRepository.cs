﻿using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<OrderSummaryDto> GetOrdersReceivable(FiltersOrdersReceivable filters);
    }
}