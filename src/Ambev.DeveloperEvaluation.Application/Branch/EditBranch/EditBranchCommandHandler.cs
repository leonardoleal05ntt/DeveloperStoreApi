using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branch.EditBranch
{
    public class EditBranchCommandHandler : IRequestHandler<EditBranchCommand>
    {
        private readonly IBranchRepository _branchRepository;
        public EditBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task Handle(EditBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new EditBranchValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingBranch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingBranch == null)
                throw new InvalidOperationException($"Branch with id {command.Id} does not exist");

            existingBranch.Edit(command.Name);
            await _branchRepository.UpdateAsync(existingBranch);
        }
    }
}
