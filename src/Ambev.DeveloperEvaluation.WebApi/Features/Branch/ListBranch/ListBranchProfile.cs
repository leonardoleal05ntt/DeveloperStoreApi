using Ambev.DeveloperEvaluation.Application.Branch.ListBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.ListBranch
{
    public class ListBranchProfile : Profile
    {
        public ListBranchProfile()
        {
            CreateMap<ListBranchRequest, ListBranchCommand>();
            CreateMap<ListBranchResult, ListBranchResponse>();
        }
    }
}
