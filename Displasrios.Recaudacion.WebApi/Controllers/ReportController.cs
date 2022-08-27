using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ReportController(ISaleRepository saleRepository)
        {
            _rpsSales = saleRepository;
        }


        /// <summary>
        /// Obtiene los ingresos por vendedor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetIncomePerSellers()
        {
            var response = new Response<IEnumerable<string>>(true, "OK");

            try
            {
                response.Data = _rpsSales.GetIncomePerSellers();
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
