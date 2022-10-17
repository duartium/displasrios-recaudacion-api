using Displasrios.Recaudacion.Core.Constants;
using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Contracts.Services;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Sales;
using Displasrios.Recaudacion.WebApi.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/sales")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class SaleController : BaseApiController<SaleController>
    {
        private readonly ISaleRepository _rpsSale;
        private readonly IHubContext<OrderHub> _hubOrder;
        private readonly IOrderRepository _rpsOrder;
        private readonly IEmailService _srvEmail;
        public SaleController(ISaleRepository saleRepository, IHubContext<OrderHub> hubContext,
            IOrderRepository orderRepository, IEmailService emailService)
        {
            _rpsSale = saleRepository;
            _hubOrder = hubContext;
            _rpsOrder = orderRepository;
            _srvEmail = emailService;
        }

        /// <summary>
        /// Registra un nuevo pedido
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] FullOrderDto order)
        {
            var response = new Response<SaleCreated>(true, "OK");

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

                var respSale = _rpsSale.Create(order);

                if (respSale.OrderNumber <= 0)
                    return Ok(response.Update(false, "Lo sentimos, no se pudo procesar la venta.", null));

                var summaryOrder = _rpsOrder.GetSummaryOrder(respSale.OrderNumber);
                response.Data = respSale;

                _hubOrder.Clients.All.SendAsync("orderentry", JsonSerializer.Serialize(summaryOrder));

                //Se envía email desde aquí
                if (respSale.SendEmail)
                    SendReceipt(respSale.OrderNumber);
                
                return Created("http://localhost:63674/api/v1/sales/", response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.InnerException != null ? "INNER EX: " + ex.InnerException.ToString() : "EXCEPTION:" + ex.ToString());
                return Conflict(response.Update(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message, null));
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
                Logger.LogError(ex.InnerException != null ? "INNER EX: " + ex.InnerException.ToString() : "EXCEPTION:" + ex.ToString());
                return Conflict(response.Update(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message, false));
            }
        }


        /// <summary>
        /// Envía al cliente el recibo de compra por correo electrónico
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        [HttpPost("send-receipt")]
        public IActionResult SendReceipt([FromBody] int orderNumber)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                
                string emailString = _rpsSale.GetSaleTemplateForEmail(orderNumber);
                
                var mailSettings = Configuration.GetSection("MailSettings").Get<MailSettings>();

                if (mailSettings.SendEmail.Equals("N"))
                    return Ok(response.Update(true, "Envío de email está desactivado (N)", false));

                string emailAddress = mailSettings.EmailTo;
                
                if (mailSettings.SendEmailToMe.Equals("N"))
                    emailAddress = _rpsSale.GetEmailFromInvoice(orderNumber);
                
                string responseEmail = "";

                _srvEmail.Send(new EmailParams
                {
                    SenderEmail = "asistencia@displasrios.com",
                    SenderName = "DISPLASRIOS S.A.",
                    Subject = "Ha recibido un comprobante de pago",
                    EmailTo = emailAddress,
                    Body = emailString
                }, out responseEmail);

                Logger.LogError($"Respuesta email comprobante de pago: idInvoice: ${orderNumber} | " + responseEmail);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.InnerException != null ? "INNER EX: " + ex.InnerException.ToString() : "EXCEPTION:" + ex.ToString());
                return Conflict(response.Update(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message, false));
            }
        }

    }
}
