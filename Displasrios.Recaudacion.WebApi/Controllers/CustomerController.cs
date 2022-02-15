using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : BaseApiController<CustomerController>
    {
        private readonly ICustomerRepository _rpsCustomer;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _rpsCustomer = customerRepository;
        }


        /// <summary>
        /// Obtiene un cliente por identificación
        /// </summary>
        /// <param name="identification"></param>
        /// <returns></returns>
        [HttpGet("{identification}")]
        public IActionResult GetCustomer(string identification)
        {
            try
            {
                var customers = _rpsCustomer.GetByIdentification(identification);
                if (customers == null)
                    return NotFound("El cliente no existe.");

                return Ok(customers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict();
            }
        }

        /// <summary>
        /// Obtiene un cliente por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            try
            {
                var customer = _rpsCustomer.Get(id);
                if (customer == null)
                    return NotFound("El cliente no existe.");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict();
            }
        }

        /// <summary>
        /// Obtener una lista de clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                var customers = _rpsCustomer.GetAll();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict();
            }
        }

    }
}
