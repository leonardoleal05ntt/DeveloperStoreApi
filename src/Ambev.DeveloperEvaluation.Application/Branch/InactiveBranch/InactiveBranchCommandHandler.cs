using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch
{
    public class InactiveBranchCommandHandler : IRequestHandler<InactiveBranchCommand>
    {
        private readonly IBranchRepository _branchRepository;
        public InactiveBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task Handle(InactiveBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new InactiveBranchValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingBranch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingBranch == null)
                throw new InvalidOperationException($"Branch with id {command.Id} does not exist");

            existingBranch.Inactive();
            await _branchRepository.UpdateAsync(existingBranch);
        }
    }
}
