﻿using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
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
            var response = new Response<ProductDto>(true, "OK");

            try
            {
                var products = _rpsProduct.GetAll();
                if (products == null)
                    return NotFound(response.Update(false, "No se encontraron productos.", null));

                return Ok(products);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Obtener una lista de productos
        /// /// <param name="id"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var response = new Response<ProductDto>(true, "OK");

            try
            {
                var product = _rpsProduct.GetById(id);
                if (product == null)
                    return NotFound(response.Update(false, "No se encontraró el producto.", null));

                return Ok(product);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return Conflict(response.Update(false, ex.Message, null));
            }
        }

    }
}