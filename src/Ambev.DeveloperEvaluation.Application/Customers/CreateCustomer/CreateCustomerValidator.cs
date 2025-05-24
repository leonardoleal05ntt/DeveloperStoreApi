using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
            RuleFor(user => user.DocumentNumber).NotEmpty().Length(3, 50);
        }
    }
}
