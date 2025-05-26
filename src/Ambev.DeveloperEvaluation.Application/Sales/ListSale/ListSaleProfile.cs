using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleProfile : Profile
    {
        public ListSaleProfile()
        {
            CreateMap<Sale, ListSaleResult>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.TotalSale, opt => opt.MapFrom(src => src.Total)) 
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, ListSaleItemResult>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))         
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))      
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))         
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));             
        }
    }
}
