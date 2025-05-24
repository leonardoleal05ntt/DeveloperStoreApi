﻿using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct
{
    public class GetProductProfile : Profile
    {
        public GetProductProfile()
        {
            CreateMap<Guid, GetProductCommand>()
                .ConstructUsing(id => new GetProductCommand(id));
            CreateMap<GetProductResult, GetProductResponse>();
        }
    }
}
