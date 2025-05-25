using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.EditBranch
{
    public class EditBranchRequestValidator : AbstractValidator<EditBranchRequest>
    {
        public EditBranchRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
        }
    }
}
