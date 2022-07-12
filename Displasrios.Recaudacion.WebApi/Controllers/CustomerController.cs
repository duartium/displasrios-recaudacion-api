using Displasrios.Recaudacion.Core.Contracts.Repositories;
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
    [Route("api/v{version:apiVersion}/customers")]
    [ApiController]
    [ApiVersion("1.0")]
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
        [HttpGet("identification/{identification}")]
        public IActionResult GetCustomer(string identification)
        {
            var response = new Response<CustomerSearchOrderDto>(true, "OK");

            try
            {
                var customers = _rpsCustomer.GetByIdentification(identification);
                if (customers == null)
                    return NotFound(response.Update(false, "No se encontró el cliente.", null));

                response.Data = customers;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Obtiene un cliente por identificación
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        [HttpGet("names/{names}")]
        public IActionResult GetCustomerByNames(string names)
        {
            var response = new Response<CustomerSearchOrderDto[]>(true, "OK");

            try
            {
                var customers = _rpsCustomer.GetByNames(names);
                if (customers == null)
                    return NotFound(response.Update(false, "No se encontraron clientes.", null));

                response.Data = customers;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
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
            var response = new Response<CustomerDto>(true, "OK");

            try
            {
                var customer = _rpsCustomer.Get(id);
                if (customer == null)
                    return NotFound(response.Update(false, "No se encontró el cliente.", null));

                response.Data = customer;
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtener una lista de clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var response = new Response<IEnumerable<CustomerDto>>(true, "OK");

            try
            {
                var customers = _rpsCustomer.GetAll();

                if (customers.ToList().Count == 0)
                    return NotFound(response.Update(false, "No se encontraron clientes.", null));

                response.Data = customers;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Actualiza la información de un cliente
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] CustomerUpdate customer)
        {
            var response = new Response<string>(true, "OK");

            try
            {
                if (!_rpsCustomer.Update(customer))
                    return Ok(response.Update(false, "Lo sentimos, no se pudo actualizar el cliente.", null));

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
