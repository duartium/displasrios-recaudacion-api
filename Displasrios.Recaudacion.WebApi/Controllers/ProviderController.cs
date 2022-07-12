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
    [Route("api/v{version:apiVersion}/providers")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProviderController : BaseApiController<ProviderController>
    {
        private readonly IProviderRepository _rpsProvider;
        public ProviderController(IProviderRepository providerRepository)
        {
            _rpsProvider = providerRepository;
        }

        /// <summary>
        /// Obtiene una lista de proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProviders()
        {
            var response = new Response<IEnumerable<ProviderDto>>(true, "OK");

            try
            {
                response.Data = _rpsProvider.GetAll(); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene una lista de proveedores en formato de catálogo
        /// </summary>
        /// <returns></returns>
        [HttpGet("catalogue")]
        public IActionResult GetAsCatalogue()
        {
            var response = new Response<IEnumerable<ItemCatalogueDto>>(true, "OK");

            try
            {
                response.Data = _rpsProvider.GetAsCatalogue(); ;
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
