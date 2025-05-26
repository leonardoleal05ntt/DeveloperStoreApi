using Ambev.DeveloperEvaluation.Application.Products.InactiveProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.InactiveProduct
{
    public class InactiveProductProfile : Profile
    {
        public InactiveProductProfile()
        {
            CreateMap<InactiveProductRequest, InactiveProductCommand>();
        }
    }
}
