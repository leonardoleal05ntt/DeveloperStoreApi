using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer
{
    public class GetCustomerCommand : IRequest<GetCustomerResult>
    {
        public Guid Id { get; }

        public GetCustomerCommand(Guid id)
        {
            Id = id;
        }
    }
}
