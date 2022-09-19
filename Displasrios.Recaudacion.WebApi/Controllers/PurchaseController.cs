using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/purchases")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class PurchaseController : BaseApiController<PurchaseController>
    {
        private readonly IPurchaseRepository _rpsPurchase;
        public PurchaseController(IPurchaseRepository purchaseRepository)
        {
            _rpsPurchase = purchaseRepository;
        }


        /// <summary>
        /// Registra una nueva compra
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] PurchaseCreate purchase)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                purchase.UserCreation = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                if (!_rpsPurchase.Register(purchase))
                    response.Update(false, "No se pudo registrar la compra.", null);

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
