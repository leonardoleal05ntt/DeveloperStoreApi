using Ambev.DeveloperEvaluation.Application.Customers.EditCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.EditCustomer
{
    public class EditCustomerProfile : Profile
    {
        public EditCustomerProfile()
        {
            CreateMap<EditCustomerRequest, EditCustomerCommand>();
        }
    }
}
