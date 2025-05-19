using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {

        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number cannot be empty");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer cannot be empty");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch cannot be empty");
            RuleFor(x => x.Items).NotEmpty().WithMessage("The sale must contain at least one item");
            RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}
