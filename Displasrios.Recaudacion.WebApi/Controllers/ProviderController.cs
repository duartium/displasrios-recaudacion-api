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

        /// <summary>
        /// Crea un nuevo proveedor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ProviderCreate provider) {
            var response = new Response<string>(true, "OK");

            try
            {
                provider.UserCreation = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                if (!_rpsProvider.Create(provider))
                    response.Update(false, "No se pudo crear el proveedor.", null);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Elimina un proveedor por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                if (!_rpsProvider.Remove(id))
                    return BadRequest(response.Update(false, "No se pudo eliminar el proveedor.", null));

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
