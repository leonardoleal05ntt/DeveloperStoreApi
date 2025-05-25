using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.ListProduct
{
    public class ListProductRequestValidator : AbstractValidator<ListProductRequest>
    {
        public ListProductRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
