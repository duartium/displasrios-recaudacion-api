using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController<ProductController>
    {
        private readonly IProductRepository _rpsProduct;
        public ProductController(IProductRepository productRepository)
        {
            _rpsProduct = productRepository;
        }


    }
}
