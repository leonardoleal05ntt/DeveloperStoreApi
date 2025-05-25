using Ambev.DeveloperEvaluation.Application.Products.EditProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.EditProduct
{
    public class EditProductProfile : Profile
    {
        public EditProductProfile()
        {
            CreateMap<EditProductRequest, EditProductCommand>();
        }
    }
}
