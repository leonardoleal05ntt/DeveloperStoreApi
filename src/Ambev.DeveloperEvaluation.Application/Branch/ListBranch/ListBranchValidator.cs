using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branch.ListBranch
{
    public class ListBranchValidator : AbstractValidator<ListBranchCommand>
    {
        public ListBranchValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
