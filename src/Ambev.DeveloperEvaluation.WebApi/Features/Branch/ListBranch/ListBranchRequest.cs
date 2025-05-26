namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.ListBranch
{
    public class ListBranchRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool? Active { get; set; }
    }
}
