using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/reports")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ReportController : BaseApiController<ReportController>
    {
        private readonly ISaleRepository _rpsSales;
        private readonly IOrderRepository _rpsOrders;

        public ReportController(ISaleRepository saleRepository, IOrderRepository orderRepository)
        {
            _rpsSales = saleRepository;
            _rpsOrders = orderRepository;
        }


        /// <summary>
        /// Obtiene los ingresos por vendedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("income-per-sellers")]
        public IActionResult GetIncomePerSellers([FromQuery] IncomeBySellers incomeBySellers)
        {
            var response = new Response<IEnumerable<IncomeBySellersDto>>(true, "OK");

            try
            {
                response.Data = _rpsSales.GetIncomePerSellers(incomeBySellers);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene total de ingresos diarios en el día de vendedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-total-sales-today")]
        public IActionResult GetTotal()
        {
            var response = new Response<decimal>(true, "OK");

            try
            {
                int idUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
                response.Data = _rpsOrders.GetTotalSalesTodayBySeller(idUser);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, 0));
            }
        }

    }
}
