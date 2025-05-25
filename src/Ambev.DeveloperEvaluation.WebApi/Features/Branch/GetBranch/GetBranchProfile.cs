using Ambev.DeveloperEvaluation.Application.Branch.GetBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.GetBranch
{
    public class GetBranchProfile : Profile
    {
        public GetBranchProfile()
        {
            CreateMap<Guid, GetBranchCommand>()
                .ConstructUsing(id => new GetBranchCommand(id));
            CreateMap<GetBranchResult, GetBranchResponse>();
        }
    }
}
