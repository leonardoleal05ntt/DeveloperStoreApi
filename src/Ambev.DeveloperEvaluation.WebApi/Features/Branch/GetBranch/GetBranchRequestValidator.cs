using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.GetBranch
{
    public class GetBranchRequestValidator : AbstractValidator<GetBranchRequest>
    {
        public GetBranchRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
