using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate(UserLogin user)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Conflict();
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Conflict();
            }
        }
    }
}