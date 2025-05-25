using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.CreateBranch
{
    public class CreateBranchCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}
