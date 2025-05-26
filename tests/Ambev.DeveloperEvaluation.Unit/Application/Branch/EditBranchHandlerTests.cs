using Ambev.DeveloperEvaluation.Application.Branch.EditBranch;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branch
{
    public class EditBranchHandlerTests
    {
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly EditBranchCommandHandler _handler;

        public EditBranchHandlerTests()
        {
            _mockBranchRepository = new Mock<IBranchRepository>();
            _handler = new EditBranchCommandHandler(_mockBranchRepository.Object);
        }


        [Fact(DisplayName = "Dado comando válido e filial existente, quando editar filial, então atualiza filial e não lança exceção")]
        public async Task Handle_ValidCommandAndBranchExists_UpdatesBranchAndDoesNotThrow()
        {
            // Arrange
            var command = EditBranchCommandTestData.GenerateValidCommand();
            var originalBranchName = "Original Branch Name";

            var existingBranch = new DeveloperEvaluation.Domain.Entities.Branch(originalBranchName);

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.Is<CancellationToken>(c => true)))
                                 .ReturnsAsync(existingBranch);

            _mockBranchRepository.Setup(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.Is<CancellationToken>(c => true)))
                                 .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            _mockBranchRepository.Verify(r => r.GetByIdAsync(command.Id, It.Is<CancellationToken>(c => true)), Times.Once);

            _mockBranchRepository.Setup(r => r.UpdateAsync(It.IsAny<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask);
        }

        [Fact(DisplayName = "Dado comando com nome vazio, quando editar filial, então lança ValidationException")]
        public async Task Handle_InvalidCommand_EmptyName_ThrowsValidationException()
        {
            // Arrange
            var command = EditBranchCommandTestData.GenerateCommandWithEmptyName();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *'Name' must not be empty.*");

            _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.Is<CancellationToken>(c => true)), Times.Never);
        }

        [Fact(DisplayName = "Dado comando com ID vazio, quando editar filial, então lança ValidationException")]
        public async Task Handle_InvalidCommand_EmptyId_ThrowsValidationException()
        {
            // Arrange
            var command = EditBranchCommandTestData.GenerateCommandWithEmptyId();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                        .WithMessage("Validation failed: *'Id' must not be empty.*");

            _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.Is<CancellationToken>(c => true)), Times.Never);
        }

        [Fact(DisplayName = "Dado filial inexistente, quando editar filial, então lança InvalidOperationException")]
        public async Task Handle_BranchDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = EditBranchCommandTestData.GenerateValidCommand();

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.Is<CancellationToken>(c => true)))
                                 .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Branch)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Branch with id {command.Id} does not exist");

            _mockBranchRepository.Verify(r => r.UpdateAsync(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>(), It.Is<CancellationToken>(c => true)), Times.Never);
        }
    }
}
