using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomer
{
    public class ListCustomerRequestValidator : AbstractValidator<ListCustomerRequest>
    {
        public ListCustomerRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
