using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branch.GetBranch
{
    public class GetBranchValidator : AbstractValidator<GetBranchCommand>
    {
        public GetBranchValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
