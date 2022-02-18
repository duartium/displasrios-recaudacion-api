using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/catalogs")]
    [ApiController]
    public class CatalogueController : BaseApiController<CatalogueController>
    {
        private readonly ICatalogueRepository _rpsCatalogue;

        public CatalogueController(ICatalogueRepository catalogueRepository)
        {
            _rpsCatalogue = catalogueRepository;
        }

        /// <summary>
        /// Obtiene un cliente por identificación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCatalogs()
        {
            var response = new Response<List<CatalogueDto>>(true, "OK");

            try
            {
                var catalogues = _rpsCatalogue.GetAll();
                if (catalogues == null || catalogues.Count == 0)
                    return NotFound(response.Update(false, "No se encontraron catálogos.", null));

                response.Data = catalogues;
                return Ok(catalogues);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }
    }
}
