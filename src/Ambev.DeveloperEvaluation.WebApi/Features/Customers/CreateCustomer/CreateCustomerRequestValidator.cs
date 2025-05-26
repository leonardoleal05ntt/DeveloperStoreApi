using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
            RuleFor(user => user.DocumentNumber).NotEmpty().Length(3, 50);
        }
    }
}
