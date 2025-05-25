using Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.InactiveBranch
{
    public class InactiveBranchProfile : Profile
    {
        public InactiveBranchProfile()
        {
            CreateMap<InactiveBranchRequest, InactiveBranchCommand>();
        }
    }
}
