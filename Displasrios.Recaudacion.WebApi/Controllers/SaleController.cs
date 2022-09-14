using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/sales")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class SaleController : BaseApiController<SaleController>
    {
        private readonly ISaleRepository _rpsSale;
        public SaleController(ISaleRepository saleRepository)
        {
            _rpsSale = saleRepository;
        }

        /// <summary>
        /// Registra un nuevo pedido
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] FullOrderDto order)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                if(order.IdClient <= 0)
                    return BadRequest(response.Update(false, "Se esperaba el id del cliente.", null));

                if (order.PaymentMethod <= 0)
                    return BadRequest(response.Update(false, "La forma de pago es obligatoria.", null));

                if (order.Items == null || order.Items.Count == 0)
                    return BadRequest(response.Update(false, "Se esperaba al menos 1 producto para procesar la venta.", null));

                if (order.Subtotal == 0)
                    return BadRequest(response.Update(false, "El subtotal debe ser mayor a cero.", null));
                
                if (order.Total == 0)
                    return BadRequest(response.Update(false, "El total debe ser mayor a cero.", null));


                order.IdUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
                order.Username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                int resp =_rpsSale.Create(order);

                if (resp <= 0)
                    return Ok(response.Update(false, "Lo sentimos, no se pudo procesar la venta.", null));

                response.Data = resp.ToString().PadLeft(5, '0');
                return Created("http://localhost:63674/api/v1/sales/1", response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Registra las ventas del día del vendedor previo al cierre de caja del administrador de ventas
        /// </summary>
        /// <param name="salesSellerToday"></param>
        /// <returns></returns>
        [HttpPost("save-collector-sales")]
        public IActionResult SaveCollectorSales([FromBody] SalesSellerToday salesSellerToday)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                salesSellerToday.Username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                response.Data = _rpsSale.SaveCollectorSale(salesSellerToday);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }

    }
}
