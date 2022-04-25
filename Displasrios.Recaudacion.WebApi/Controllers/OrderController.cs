using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/order")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseApiController<OrderController>
    {
        private readonly IOrderRepository _rpsOrder;
        public OrderController(IOrderRepository order)
        {
            _rpsOrder = order;
        }

        [HttpGet("receivable")]
        public IActionResult GetOrdersReceivable()
        {
            //[FromBody] FiltersOrdersReceivable filters
            var response = new Response<IEnumerable<OrderSummaryDto>>(true, "OK");

            try
            {
                response.Data = _rpsOrder.GetOrdersReceivable(new FiltersOrdersReceivable());
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        [HttpGet("receivable/{id}")]
        public IActionResult GetOrderReceivable(int id)
        {
            var response = new Response<OrderReceivableDto>(true, "OK");

            try
            {
                response.Data = _rpsOrder.GetOrderReceivable(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

    }
}
