using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branch.GetBranch
{
    public class GetBranchProfile : Profile
    {
        public GetBranchProfile()
        {
            CreateMap<Domain.Entities.Branch, GetBranchResult>();
        }
    }
}
