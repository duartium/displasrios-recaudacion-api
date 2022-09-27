using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.DTOs.Reports;
using Displasrios.Recaudacion.Core.DTOs.Sales;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        private readonly ICustomerRepository _rpsCustomer;
        private readonly IProductRepository _rpsProduct;

        public ReportController(ISaleRepository saleRepository, IOrderRepository orderRepository, 
            ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _rpsSales = saleRepository;
            _rpsOrders = orderRepository;
            _rpsCustomer = customerRepository;
            _rpsProduct = productRepository;
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
        public IActionResult GetTotalSalesToday()
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

        /// <summary>
        /// Obtiene resumen (totales) para presentación en pantalla de mi reporte del vendedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-seller-personal-report")]
        public IActionResult GetSellerPersonalReport()
        {
            var response = new Response<SellerPersonalReportDto>(true, "OK");

            try
            {
                int idUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
                response.Data = _rpsOrders.GetSellerPersonalReport(idUser);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene los 7 mejores clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet("best-customers")]
        public IActionResult GetBestCustomers()
        {
            var response = new Response<IEnumerable<BestCustomerDto>>(true, "OK");

            try
            {
                response.Data = _rpsCustomer.GetBestCustomers();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene los 10 productos más vendidos
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-selled-products")]
        public IActionResult GetMostSelledProducts()
        {
            var response = new Response<IEnumerable<MostSelledProductDto>>(true, "OK");

            try
            {
                response.Data = _rpsProduct.GetMostSelledProducts();
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
