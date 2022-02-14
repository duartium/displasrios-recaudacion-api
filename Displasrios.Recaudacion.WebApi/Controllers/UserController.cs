using Displasrios.Recaudacion.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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


        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _rpsUser.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict();
            }
        }
    }
}