using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.EditSale
{
    public class EditSaleItemRequestValidatior : AbstractValidator<EditSaleItemRequest>
    {
        public EditSaleItemRequestValidatior()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product cannot be empty");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("The unit price must be greater than zero");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The quantity must be greater than zero")
                .LessThanOrEqualTo(20).WithMessage("The maximum quantity per item is 20");
        }

    }
}
