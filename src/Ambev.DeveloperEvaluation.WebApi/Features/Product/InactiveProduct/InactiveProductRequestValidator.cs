using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.InactiveProduct
{
    public class InactiveProductRequestValidator : AbstractValidator<InactiveProductRequest>
    {
        public InactiveProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
