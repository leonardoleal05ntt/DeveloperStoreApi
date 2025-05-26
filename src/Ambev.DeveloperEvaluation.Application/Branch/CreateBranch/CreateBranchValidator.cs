using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branch.CreateBranch
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchValidator()
        {
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
        }
    }
}
