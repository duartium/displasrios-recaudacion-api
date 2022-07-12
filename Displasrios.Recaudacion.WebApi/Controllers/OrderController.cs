using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/order")]
    [ApiController]
    [ApiVersion("1.0")]
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

        [HttpPost("receivable/pay")]
        public IActionResult RegisterPayment([FromBody] OrderReceivableCreateRequest order_payment)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                string message = String.Empty;
                _rpsOrder.RegisterPayment(order_payment, out message);

                response.Message = message;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        [HttpGet("orders-of-day")]
        public IActionResult GetOrdersOfDay()
        {
            var response = new Response<IEnumerable<SummaryOrdersOfDay>>(true, "OK");

            try
            {
                response.Data =_rpsOrder.GetSummaryOrdersOfDay();
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
