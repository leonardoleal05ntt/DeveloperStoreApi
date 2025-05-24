using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Guid> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateCustomerValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCustomer = await _customerRepository.GetByDocumentNumberAsync(command.DocumentNumber, cancellationToken);
            if (existingCustomer != null)
                throw new InvalidOperationException($"Customer with document number {command.DocumentNumber} already exists");

            var customer = new Customer(command.Name, command.DocumentNumber);
            await _customerRepository.CreateAsync(customer);
            return customer.Id;
        }
    }
}
