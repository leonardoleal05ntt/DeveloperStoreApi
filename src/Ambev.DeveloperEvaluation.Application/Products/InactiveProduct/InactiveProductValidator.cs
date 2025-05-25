using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.InactiveProduct
{
    public class InactiveProductValidator : AbstractValidator<InactiveProductCommand>
    {
        public InactiveProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
