using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleValidator : AbstractValidator<ListSaleCommand>
    {
        public ListSaleValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
