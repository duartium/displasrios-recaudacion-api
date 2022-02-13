using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _srvAuthentication;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _srvAuthentication = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public ActionResult Authenticate([FromBody] UserLogin request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Los datos de entrada son inválidos");

                string token;
                if (!_srvAuthentication.IsAuthenticated(request, out token))
                    return BadRequest("Invalid Request");

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Conflict(ex.Message);
            }
        }

    }
}