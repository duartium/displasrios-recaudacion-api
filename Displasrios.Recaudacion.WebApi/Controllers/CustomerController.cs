using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        /// Obtiene deudas del cliente por identificación
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        [HttpGet("debts")]
        public IActionResult GetDebts(string identification, string names)
        {
            var response = new Response<CustomerDebtDto>(true, "OK");

            try
            {
                var debts = _rpsCustomer.GetDebts(identification, names);
                if (debts == null)
                    return NotFound(response.Update(false, "No se encontraron deudas.", null));

                response.Data = debts;
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
        [HttpPut]
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

        /// <summary>
        /// Registra un nuevo cliente
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] CustomerCreate customer)
        {
            var response = new Response<int>(true, "Creado");

            try
            {
                var customerValidator = new CustomerCreationValidator().Validate(customer);
                if (!customerValidator.IsValid)
                    return BadRequest(response.Update(false, JsonConvert.SerializeObject(customerValidator.Errors.Select(x => x.ErrorMessage).ToArray()), -1));


                customer.CurrentUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                int idCustomer = _rpsCustomer.Create(customer);
                if (idCustomer <= 0)
                    response.Update(false, "No se pudo registrar el cliente.", -1);

                response.Data = idCustomer;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, -1));
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var response = new Response<int>(true, "OK");

            try
            {
                if(!_rpsCustomer.Delete(id))
                    response.Update(false, "No se pudo eliminar el cliente.", -1);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, -1));
            }
        }

    }
}
