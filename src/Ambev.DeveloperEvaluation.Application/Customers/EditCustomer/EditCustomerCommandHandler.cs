using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.EditCustomer
{
    public class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        public EditCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(EditCustomerCommand command, CancellationToken cancellationToken)
        {
            var validator = new EditCustomerValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCustomer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingCustomer == null)
                throw new InvalidOperationException($"Customer with id {command.Id} does not exist");

            var existingCustomerWithDocument = await _customerRepository.GetByDocumentNumberAsync(command.DocumentNumber, cancellationToken);
            if (existingCustomerWithDocument != null && existingCustomerWithDocument.Id != command.Id)
                throw new InvalidOperationException($"Customer with document number {command.DocumentNumber} already exists");

            existingCustomer.Edit(command.Name, command.DocumentNumber);
            await _customerRepository.UpdateAsync(existingCustomer);
        }
    }
}
