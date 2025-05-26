using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers
{
    public class ListCustomerValidator : AbstractValidator<ListCustomerCommand>
    {
        public ListCustomerValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
