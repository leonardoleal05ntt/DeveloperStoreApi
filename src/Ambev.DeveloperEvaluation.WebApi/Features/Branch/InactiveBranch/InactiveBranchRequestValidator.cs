using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.InactiveBranch
{
    public class InactiveBranchRequestValidator : AbstractValidator<InactiveBranchRequest>
    {
        public InactiveBranchRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
