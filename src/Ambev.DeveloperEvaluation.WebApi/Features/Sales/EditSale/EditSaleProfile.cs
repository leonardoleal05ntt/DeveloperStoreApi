using Ambev.DeveloperEvaluation.Application.Sales.EditSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.EditSale
{
    public class EditSaleProfile : Profile
    {
        public EditSaleProfile()
        {
            CreateMap<EditSaleRequest, EditSaleCommand>();
            CreateMap<EditSaleItemRequest, EditSaleItemDto>();
        }
    }
}
