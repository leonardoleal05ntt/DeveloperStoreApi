using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branch.ListBranch
{
    public class ListBranchProfile : Profile
    {
        public ListBranchProfile()
        {
            CreateMap<Domain.Entities.Branch, ListBranchResult>();
        }
    }
}
