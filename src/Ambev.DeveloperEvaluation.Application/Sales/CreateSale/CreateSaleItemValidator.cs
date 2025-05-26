using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemDto>
    {
        public CreateSaleItemValidator() 
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product cannot be empty");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("The unit price must be greater than zero");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The quantity must be greater than zero")
                .LessThanOrEqualTo(20).WithMessage("The maximum quantity per item is 20");
        }

    }
}
