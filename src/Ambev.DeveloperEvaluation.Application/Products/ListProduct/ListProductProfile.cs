using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductProfile : Profile
    {
        public ListProductProfile()
        {
            CreateMap<Product, ListProductResult>();
        }
    }
}
