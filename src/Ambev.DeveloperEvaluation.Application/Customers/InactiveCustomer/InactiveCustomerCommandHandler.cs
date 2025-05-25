using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.InactiveCustomer
{
    internal class InactiveCustomerCommandHandler : IRequestHandler<InactiveCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        public InactiveCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(InactiveCustomerCommand command, CancellationToken cancellationToken)
        {
            var validator = new InactiveCustomerValidiator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCustomer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingCustomer == null)
                throw new InvalidOperationException($"Customer with id {command.Id} does not exist");

            existingCustomer.Inactive();
            await _customerRepository.UpdateAsync(existingCustomer);
        }
    }
}
