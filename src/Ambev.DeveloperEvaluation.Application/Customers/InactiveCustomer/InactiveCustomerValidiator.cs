using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.InactiveCustomer
{
    public class InactiveCustomerValidiator : AbstractValidator<InactiveCustomerCommand>
    {
        public InactiveCustomerValidiator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
