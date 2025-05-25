using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomer
{
    public class ListCustomerProfile : Profile
    {
        public ListCustomerProfile()
        {
            CreateMap<ListCustomerRequest, ListCustomerCommand>();
            CreateMap<ListCustomerResult, ListCustomerResponse>();
        }
    }
}
