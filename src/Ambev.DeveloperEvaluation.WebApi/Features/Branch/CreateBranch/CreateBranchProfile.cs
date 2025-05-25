using Ambev.DeveloperEvaluation.Application.Branch.CreateBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.CreateBranch
{
    public class CreateBranchProfile : Profile
    {
        public CreateBranchProfile()
        {
            CreateMap<CreateBranchRequest, CreateBranchCommand>();
        }
    }
}
