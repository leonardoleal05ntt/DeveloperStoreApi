using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.InactiveCustomer
{
    public class InactiveCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
