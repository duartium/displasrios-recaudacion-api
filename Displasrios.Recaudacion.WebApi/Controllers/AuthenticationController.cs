using Displasrios.Recaudacion.Core.Constants;
using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Contracts.Services;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neutrinodevs.PedidosOnline.Infraestructure.Security;
using System;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/authentication")]
    [ApiController]
    [ApiVersion("1.0")]

    public class AuthenticationController : BaseApiController<AuthenticationController>
    {

        private readonly IAuthenticationService _srvAuthentication;
        private readonly IEmailService _srvEmail;
        private readonly IUserRepository _rpsUser;

        public AuthenticationController(IAuthenticationService authenticationService, IEmailService emailService,
            IUserRepository userRepository)
        {
            _srvAuthentication = authenticationService;
            _srvEmail = emailService;
            _rpsUser = userRepository;
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

        [AllowAnonymous]
        [HttpPost, Route("forgot-password")]
        public ActionResult ForgotPassword([FromBody] ForgotPassword request)
        {
            var response = new Response<string>(true, "OK");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.Update(false, "El correo electrónico es obligatorio", null));

                if (!_rpsUser.Exists(request.Email))
                    return BadRequest(response.Update(false, "El correo electrónico no se encuentra vinculado a una cuenta.", null));

                string code = VerificationCode.Generate(5);
                _rpsUser.RegisterVerificationCode(request.Email, code.ToUpper());
                
                string bodyHtml = CString.VERIFICACION_CODE_TEMPLATE.Replace("@code", code);
                string responseEmail = "";

                _srvEmail.Send(new EmailParams
                {
                    SenderEmail = "asistencia@displasrios.com",
                    SenderName = "DISPLASRIOS S.A.",
                    Subject = "RECUPERACIÓN DE CONTRASEÑA",
                    EmailTo = request.Email,
                    Body = bodyHtml
                }, out responseEmail);

                Logger.LogError($"Respuesta email con código de verificación: ${code} | " + responseEmail);

                response.Message = responseEmail;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("verify-code")]
        public ActionResult VerifyCode([FromBody] CodeForgotPassword request)
        {
            var response = new Response<string>(true, "OK");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.Update(false, "El correo electrónico es obligatorio", null));

                if (!_rpsUser.Exists(request.Email))
                    return BadRequest(response.Update(false, "El correo electrónico no se encuentra vinculado a una cuenta.", null));

                var verifyResponse = _rpsUser.VerifyCode(request.Email, request.Code.ToUpper());
                response.Message = verifyResponse.Message;
                response.Success = verifyResponse.IsSuccess;

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("change-password")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = new Response<string>(true, "OK");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.Update(false, "El correo electrónico es obligatorio", null));

                request.NewPassword = Security.GetSHA256(request.NewPassword);

                var verifyResponse = _rpsUser.ChangePassword(request.Email, request.NewPassword);
                response.Message = verifyResponse.Message;
                response.Success = verifyResponse.IsSuccess;

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