using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.ListBranch
{
    public class ListBranchCommand : IRequest<PagedResult<ListBranchResult>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public bool? Active { get; set; }

        public ListBranchCommand(int pageNumber, int pageSize, bool? active, string? search)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Search = search;
            Active = active;
        }
    }
}
