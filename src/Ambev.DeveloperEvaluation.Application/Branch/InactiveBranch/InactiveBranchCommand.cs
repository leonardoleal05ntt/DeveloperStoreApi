using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch
{
    public class InactiveBranchCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
