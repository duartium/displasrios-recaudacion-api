using Displasrios.Recaudacion.Core.Constants;
using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Contracts.Services;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neutrinodevs.PedidosOnline.Infraestructure.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly IUserRepository _rpsUser;
        private readonly IEmailService _srvEmail;

        public UserController(IUserRepository userRepository, IEmailService emailService)
        {
            _rpsUser = userRepository;
            _srvEmail = emailService;
        }

        /// <summary>
        /// Obtiene un usuario por id.
        /// </summary>
        /// <param name="id">Example id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var response = new Response<UserDto>(true, "OK");
            try
            {
                var user = _rpsUser.Get(id);
                if (user == null)
                    return NotFound(response.Update(false, "No se encontró el usuario.", null));

                response.Data = user;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene el perfil de un usuario.
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var response = new Response<UserDto>(true, "OK");
            try
            {
                int id = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value);
                var user = _rpsUser.Get(id);
                if (user == null)
                    return NotFound(response.Update(false, "No se encontró el usuario.", null));

                response.Data = user;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtiene el catálogo de perfiles de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("profiles")]
        public IActionResult GetProfiles()
        {
            var response = new Response<IEnumerable<ItemCatalogueDto>>(true, "OK");
            try
            {
                response.Data = _rpsUser.GetUserProfiles();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Obtener una lista de usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var response = new Response<IEnumerable<UserDto>>(true, "OK");
            try
            {
                var users = _rpsUser.GetAll();
                if (users.ToList().Count == 0)
                    return NotFound(response.Update(false, "No se encontraron usuarios.", null));

                response.Data = users;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtener una lista de todos los recaudadores que aún no han entregado sus ingresos del día para el arqueo de caja
        /// </summary>
        /// <returns></returns>
        [HttpGet("collectors-catalog")]
        public IActionResult GetCollectors()
        {
            var response = new Response<IEnumerable<CollectorResumeDto>>(true, "OK");
            try
            {
                response.Data = _rpsUser.GetCollectors();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] UserCreation user)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                var userValidation = new UserCreationValidator().Validate(user);
                if (!userValidation.IsValid)
                    return BadRequest(response.Update(false, JsonConvert.SerializeObject(userValidation.Errors.Select(x => x.ErrorMessage).ToArray()), null));
                

                user.CurrentUser = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                user.CodeEmailVerification = Password.Generate(6);
                user.Password = Security.GetSHA256(user.CodeEmailVerification);
                user.Username = _rpsUser.GenerateUsername(user.Names, user.Surnames);

                if (!_rpsUser.Create(user))
                    return Ok(response.Update(false, "Lo sentimos, no se pudo crear el usuario.", null));

                string bodyHtml = CString.EMAIL_TEMPLATE.Replace("@code", user.CodeEmailVerification);
                string responseEmail = "";

                _srvEmail.Send(new EmailParams
                {
                    SenderEmail = "asistencia@displasrios.com",
                    SenderName = "DISPLASRIOS S.A.",
                    Subject = "¡Te damos la bienvenida!",
                    EmailTo = user.Email,
                    Body = bodyHtml
                }, out responseEmail);

                Logger.LogError(JsonConvert.SerializeObject(user) + " :" + responseEmail);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.InnerException != null ? "INNER EX: "+ex.InnerException.ToString() : "EXCEPTION:" + ex.ToString());
                return Conflict(response.Update(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message, null));
            }
        }

        /// <summary>
        /// Elimina un usuario por id
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [HttpDelete("{idUser}")]
        public IActionResult Delete(int idUser) {
            var response = new Response<string>(true, "OK");
            try
            {
                if (!_rpsUser.Remove(idUser))
                    return BadRequest(response.Update(false, "No se pudo eliminar el usuario.", null));

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.InnerException != null ? "INNER EX: " + ex.InnerException.ToString() : "EXCEPTION:" + ex.ToString());
                return Conflict(response.Update(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message, null));
            }
        }

    }
}