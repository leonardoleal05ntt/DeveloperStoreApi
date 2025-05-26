using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.EditSale
{
    public class EditSaleValidator : AbstractValidator<EditSaleCommand>
    {
        public EditSaleValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number cannot be empty");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer cannot be empty");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch cannot be empty");
            RuleFor(x => x.Items).NotEmpty().WithMessage("The sale must contain at least one item");
            RuleForEach(x => x.Items).SetValidator(new EditSaleItemValidator());
        }
    }
}
