using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.EditCustomer
{
    public class EditCustomerValidator : AbstractValidator<EditCustomerCommand>
    {
        public EditCustomerValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
            RuleFor(user => user.DocumentNumber).NotEmpty().Length(3, 50);
        }
    }
}
