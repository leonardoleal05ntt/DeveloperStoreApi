using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale
{
    public class ListSaleProfile : Profile
    {
        public ListSaleProfile()
        {
            CreateMap<ListSaleResult, ListSaleResponse>();
            CreateMap<ListSaleItemResult, ListSaleItemResponse>();
            CreateMap<ListSaleRequest, ListSaleCommand>();
        }
    }
}
