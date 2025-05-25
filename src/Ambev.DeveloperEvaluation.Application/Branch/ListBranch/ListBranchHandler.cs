using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.ListBranch
{
    public class ListBranchHandler : IRequestHandler<ListBranchCommand, PagedResult<ListBranchResult>>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public ListBranchHandler(
            IBranchRepository branchRepository,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListBranchResult>> Handle(ListBranchCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListBranchValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var pagedResult = await _branchRepository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.Active, cancellationToken);

            var dtoItems = _mapper.Map<IEnumerable<ListBranchResult>>(pagedResult.Items);

            return new PagedResult<ListBranchResult>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}
