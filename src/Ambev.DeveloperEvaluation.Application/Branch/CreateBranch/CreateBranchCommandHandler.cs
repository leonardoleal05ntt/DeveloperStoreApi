using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.CreateBranch
{
    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Guid>
    {
        private readonly IBranchRepository _branchRepository;
        public CreateBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Guid> Handle(CreateBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateBranchValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var branch = new Domain.Entities.Branch(command.Name);
            await _branchRepository.CreateAsync(branch);
            return branch.Id;
        }
    }
}
