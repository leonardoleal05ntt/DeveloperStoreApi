using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.EditCustomer
{
    public class EditCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
    }
}
