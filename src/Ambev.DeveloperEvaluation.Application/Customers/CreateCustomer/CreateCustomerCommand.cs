using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
    }
}
