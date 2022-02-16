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
using Neutrinodevs.PedidosOnline.Infraestructure.Security;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : BaseApiController<AuthenticationController>
    {

        private readonly IAuthenticationService _srvAuthentication;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _srvAuthentication = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public ActionResult Authenticate([FromBody] UserLogin request)
        {
            var response = new Response<string>(true, "OK");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.Update(false, "Los datos de entrada son inválidos", null));

                string token;
                request.Password = Security.GetSHA256(request.Password);

                if (!_srvAuthentication.IsAuthenticated(request, out token))
                    return BadRequest(response.Update(false, "Usuario o contraseña incorrectas.", null));

                response.Data = token;
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