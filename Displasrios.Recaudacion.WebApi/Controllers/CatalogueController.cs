using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/catalogs")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class CatalogueController : BaseApiController<CatalogueController>
    {
        private readonly ICatalogueRepository _rpsCatalogue;

        public CatalogueController(ICatalogueRepository catalogueRepository)
        {
            _rpsCatalogue = catalogueRepository;
        }

        /// <summary>
        /// Obtiene una lista de catálogos
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene un catálogo por nombre
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public IActionResult GetCatalogue(string name)
        {
            var response = new Response<CatalogueDto>(true, "OK");

            try
            {
                var catalogue = _rpsCatalogue.Get(name);
                if (catalogue == null)
                    return NotFound(response.Update(false, "No se encontró el catálogo.", null));

                response.Data = catalogue;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Obtiene el catálogo de vendedores
        /// </summary>
        /// <returns></returns>
        [HttpGet("sellers")]
        public IActionResult GetSellers()
        {
            var response = new Response<List<ItemCatalogueDto>>(true, "OK");

            try
            {
                var catalogues = _rpsCatalogue.GetSellers().ToList();
                if (catalogues == null || catalogues.Count == 0)
                    return NotFound(response.Update(false, "No se encontraron vendedores.", null));

                response.Data = catalogues.ToList();
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
