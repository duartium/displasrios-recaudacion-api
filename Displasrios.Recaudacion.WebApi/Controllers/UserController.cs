using Displasrios.Recaudacion.Core.Contracts;
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
    [Route("api/v1/users")]
    [ApiController]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly IUserRepository _rpsUser;

        public UserController(IUserRepository userRepository)
        {
            _rpsUser = userRepository;
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
    }
}