using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.EditBranch
{
    public class EditBranchCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
