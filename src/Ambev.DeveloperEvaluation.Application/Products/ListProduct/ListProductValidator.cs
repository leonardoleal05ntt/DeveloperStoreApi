using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductValidator : AbstractValidator<ListProductCommand>
    {
        public ListProductValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
