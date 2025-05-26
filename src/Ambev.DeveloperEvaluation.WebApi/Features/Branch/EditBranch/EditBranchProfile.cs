using Ambev.DeveloperEvaluation.Application.Branch.EditBranch;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch.EditBranch
{
    public class EditBranchProfile : Profile
    {
        public EditBranchProfile()
        {
            CreateMap<EditBranchRequest, EditBranchCommand>();
        }
    }
}
