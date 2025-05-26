using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.GetBranch
{
    public class GetBranchCommand : IRequest<GetBranchResult>
    {
        public Guid Id { get; }

        public GetBranchCommand(Guid id)
        {
            Id = id;
        }
    }
}
