using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.ListBranch
{
    public class ListBranchRequestValidator : AbstractValidator<ListBranchRequest>
    {
        public ListBranchRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
