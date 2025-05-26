using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer
{
    public class GetCustomerProfile : Profile
    {
        public GetCustomerProfile()
        {
            CreateMap<Guid, GetCustomerCommand>()
                .ConstructUsing(id => new GetCustomerCommand(id));
            CreateMap<GetCustomerResult, GetCustomerResponse>();
        }
    }
}
