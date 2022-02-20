using AutoMapper;
using Displasrios.Recaudacion.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, ProductSaleDto>();
        }
        

    }
}
