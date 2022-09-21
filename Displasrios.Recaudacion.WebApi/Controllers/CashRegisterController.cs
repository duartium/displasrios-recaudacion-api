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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/cash-register")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class CashRegisterController : BaseApiController<CashRegisterController>
    {
        private readonly ICashRegisterRepository _rpsCashRepository;

        public CashRegisterController(ICashRegisterRepository cashRegister)
        {
            _rpsCashRepository = cashRegister;
        }

        /// <summary>
        /// Retorna true cuando detecta que ya se ha aperturado caja
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IsCashOpenend()
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                response.Data = _rpsCashRepository.IsOpenendCash();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }


        /// <summary>
        /// Registra apertura de caja
        /// </summary>
        /// <returns></returns>
        [HttpPost("open")]
        public IActionResult Open([FromBody] decimal initialValue)
        {
            var response = new Response<bool>(true, "Creado");

            try
            {
                int idUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

                response.Data = _rpsCashRepository.Open(initialValue, idUser); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }


        /// <summary>
        /// Registra cierre de caja
        /// </summary>
        /// <returns></returns>
        [HttpPost("close")]
        public IActionResult Close([FromBody] decimal initialValue, string observations)
        {
            var response = new Response<bool>(true, "Creado");

            try
            {
                int idUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

                response.Data = _rpsCashRepository.Close(initialValue, observations, idUser); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }


        /// <summary>
        /// Retorna los totales de ingresos por vendedores, total de ventas local, egresos y gastos 
        /// </summary>
        /// <returns></returns>
        [HttpGet("totals-cash-close")]
        public IActionResult GetTotalsForCashClose()
        {
            var response = new Response<TotalCashCloseDto>(true, "OK");

            try
            {
                response.Data = _rpsCashRepository.GetTotalsForCashClose();
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
