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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _rpsUser;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
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
                _logger.LogError(ex.ToString());
                return Conflict();
            }
        }
    }
}