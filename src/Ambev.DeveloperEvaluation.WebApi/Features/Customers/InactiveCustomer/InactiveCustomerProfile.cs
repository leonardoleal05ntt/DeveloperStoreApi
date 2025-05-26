using Ambev.DeveloperEvaluation.Application.Customers.InactiveCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.InactiveCustomer
{
    public class InactiveCustomerProfile : Profile
    {
        public InactiveCustomerProfile()
        {
            CreateMap<InactiveCustomerRequest, InactiveCustomerCommand>();
        }
    }
}
