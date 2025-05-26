using Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branch
{
    public class InactiveBranchHandlerTests
    {
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly InactiveBranchCommandHandler _handler;

        public InactiveBranchHandlerTests()
        {
            _mockBranchRepository = new Mock<IBranchRepository>();
            _handler = new InactiveBranchCommandHandler(_mockBranchRepository.Object);
        }

        [Fact(DisplayName = "Dado comando válido e filial existente e ativa, quando inativar filial, então inativa filial e não lança exceção")]
        public async Task Handle_ValidCommandAndBranchExistsAndIsActive_InactivatesBranchAndDoesNotThrow()
        {
            // Arrange
            var command = InactiveBranchCommandTestData.GenerateValidCommand();

            var existingBranch = new DeveloperEvaluation.Domain.Entities.Branch("Filial Ativa");

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(existingBranch);

            _mockBranchRepository.Setup(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            _mockBranchRepository.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);

            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Dado comando com ID vazio, quando inativar filial, então lança ValidationException")]
        public async Task Handle_InvalidCommand_EmptyId_ThrowsValidationException()
        {
            // Arrange
            var command = InactiveBranchCommandTestData.GenerateCommandWithEmptyId();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *User ID is required*");

            _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Dado filial inexistente, quando inativar filial, então lança InvalidOperationException")]
        public async Task Handle_BranchDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = InactiveBranchCommandTestData.GenerateValidCommand();

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Branch)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Branch with id {command.Id} does not exist");

            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact(DisplayName = "Dado filial existente e já inativa, quando inativar filial, então lança InvalidOperationException")]
        public async Task Handle_BranchAlreadyInactive_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = InactiveBranchCommandTestData.GenerateValidCommand();
            var existingBranch = new Ambev.DeveloperEvaluation.Domain.Entities.Branch("Filial Inativa");
            existingBranch.Inactive();

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(existingBranch);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Branch is already inactive.");

            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
