using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

                var filters = new FiltersOrdersReceivable();
                filters.IdUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

                response.Data = _rpsOrder.GetOrdersReceivable(filters);
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

        [HttpGet("collection-of-day")]
        public IActionResult GetCollectionOfDay()
        {
            var response = new Response<IEnumerable<SummaryOrdersOfDay>>(true, "OK");

            try
            {
                response.Data = _rpsOrder.GetCollectionOfDay();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        [HttpGet("orders-customer/{id}")]
        public IActionResult GetOrdersByCustomer(int id)
        {
            var response = new Response<IEnumerable<SummaryOrdersOfDay>>(true, "OK");

            try
            {
                response.Data = _rpsOrder.GetSummaryOrdersByCustomer(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        [HttpPost("record-visit")]
        public IActionResult RecordVisit([FromBody] VisitCreation visit)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                visit.Username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                response.Data = _rpsOrder.RecordVisit(visit);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }

        [HttpDelete("cancel-order/{id}")]
        public IActionResult CancelOrder(int id)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                string username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                response.Data = _rpsOrder.CancelOrder(id, username);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }

        [HttpGet("seller-debts/{id}")]
        public IActionResult GetSellerDebts(int id)
        {
            var response = new Response<CustomerDebtDto>(true, "OK");

            try
            {
                response.Data = _rpsOrder.GetSellerDebts(id);
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
