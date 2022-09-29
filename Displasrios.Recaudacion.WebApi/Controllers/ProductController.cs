using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductController : BaseApiController<ProductController>
    {
        private readonly IProductRepository _rpsProduct;
        public ProductController(IProductRepository productRepository)
        {
            _rpsProduct = productRepository;
        }

        /// <summary>
        /// Obtener una lista de productos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProducts()
        {
            var response = new Response<IEnumerable<ProductDto>>(true, "OK");

            try
            {
                var products = _rpsProduct.GetAll();
                if (products == null)
                    return NotFound(response.Update(false, "No se encontraron productos.", null));

                response.Data = products;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtener una lista de productos
        /// <param name="id"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var response = new Response<ProductDto>(true, "OK");

            try
            {
                var product = _rpsProduct.GetById(id);
                if (product == null)
                    return NotFound(response.Update(false, "No se encontró el producto.", null));

                response.Data = product;
                return Ok(product);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Obtener una lista de productos
        /// <param name="name"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-for-sale/name/{name}")]
        public IActionResult GetProducts(string name)
        {
            var response = new Response<IEnumerable<ProductSaleDto>>(true, "OK");

            try
            {
                var product = _rpsProduct.GetForSale(name);
                if (product == null)
                    return NotFound(response.Update(false, "No se encontraron productos.", null));

                response.Data = product;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtener una lista de productos
        /// <param name="id"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-for-sale/{id}")]
        public IActionResult GetBySale(int id)
        {
            var response = new Response<ProductSaleDto>(true, "OK");

            try
            {
                var product = _rpsProduct.GetById(id);
                if (product == null)
                    return NotFound(response.Update(false, "No se encontró el producto.", null));

                response.Data = Mapper.Map<ProductSaleDto>(product);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ProductCreation product) {
            var response = new Response<string>(true, "OK");

            try
            {
                if(!_rpsProduct.Create(product))
                    return NotFound(response.Update(false, "No se pudo crear el producto.", null));

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Elimina un producto por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id) 
        {
            var response = new Response<string>(true, "OK");

            try
            {
                if (!_rpsProduct.Remove(id))
                    return BadRequest(response.Update(false, "No se pudo eliminar el producto.", null));

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }


        /// <summary>
        /// Obtiene el stock actual de un producto según su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("current-stock/{id:int}")]
        public IActionResult GetCurrentStock(int id)
        {
            var response = new Response<int>(true, "OK");

            try
            {
                response.Data = _rpsProduct.GetCurrentStock(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, -1));
            }
        }

        /// <summary>
        /// Aumenta el stock de un producto por id
        /// </summary>
        /// <param name="updateStock"></param>
        /// <returns></returns>
        [HttpPost("increase-stock")]
        public IActionResult IncreaseStock([FromBody] UpdateStock updateStock)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                response.Data = _rpsProduct.IncreaseStock(updateStock.Id, updateStock.Quantity);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }

        /// <summary>
        /// Disminuye el stock de un producto por id
        /// </summary>
        /// <param name="updateStock"></param>
        /// <returns></returns>
        [HttpPost("decrease-stock")]
        public IActionResult DecreaseStock([FromBody] UpdateStock updateStock)
        {
            var response = new Response<bool>(true, "OK");

            try
            {
                response.Data = _rpsProduct.DecreaseStock(updateStock.Id, updateStock.Quantity);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, false));
            }
        }

    }
}
