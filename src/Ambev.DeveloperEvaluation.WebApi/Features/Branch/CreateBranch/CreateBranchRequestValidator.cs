using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.CreateBranch
{
    public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
    {
        public CreateBranchRequestValidator()
        {
            RuleFor(b => b.Name).NotEmpty().Length(3, 50);
        }
    }
}
