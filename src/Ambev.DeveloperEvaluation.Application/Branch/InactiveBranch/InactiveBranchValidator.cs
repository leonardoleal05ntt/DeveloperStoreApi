using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch
{
    public class InactiveBranchValidator : AbstractValidator<InactiveBranchCommand>
    {
        public InactiveBranchValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
