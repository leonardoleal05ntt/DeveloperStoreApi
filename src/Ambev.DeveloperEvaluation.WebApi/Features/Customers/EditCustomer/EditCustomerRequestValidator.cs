using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.EditCustomer
{
    public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
    {
        public EditCustomerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
            RuleFor(user => user.DocumentNumber).NotEmpty().Length(3, 50);
        }
    }
}
