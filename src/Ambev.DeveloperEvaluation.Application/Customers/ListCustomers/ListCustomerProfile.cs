using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers
{
    public class ListCustomerProfile : Profile
    {
        public ListCustomerProfile()
        {
            CreateMap<Customer, ListCustomerResult>();
        }
    }
}
