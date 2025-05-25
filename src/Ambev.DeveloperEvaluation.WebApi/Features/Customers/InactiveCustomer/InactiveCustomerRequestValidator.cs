using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.InactiveCustomer
{
    public class InactiveCustomerRequestValidator : AbstractValidator<InactiveCustomerRequest>
    {
        public InactiveCustomerRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
