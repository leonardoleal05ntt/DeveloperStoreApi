using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branch.EditBranch
{
    public class EditBranchValidator : AbstractValidator<EditBranchCommand>
    {
        public EditBranchValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().Length(3, 50);
        }
    }
}
