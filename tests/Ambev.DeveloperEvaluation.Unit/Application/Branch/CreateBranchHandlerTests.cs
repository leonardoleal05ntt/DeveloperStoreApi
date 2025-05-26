using Ambev.DeveloperEvaluation.Application.Branch.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branch
{
    public class CreateBranchHandlerTests
    {
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly CreateBranchCommandHandler _handler;

        public CreateBranchHandlerTests()
        {
            _mockBranchRepository = new Mock<IBranchRepository>();
            _handler = new CreateBranchCommandHandler(_mockBranchRepository.Object);
        }

        [Fact(DisplayName = "Dado comando válido, quando criar filial, então retorna Guid")]
        public async Task Handle_ValidCommand_ReturnsGuid()
        {
            // Arrange
            var command = CreateBranchCommandTestData.CreateValidCommand();

            _mockBranchRepository
                .Setup(r => r.CreateAsync(It.IsAny<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Ambev.DeveloperEvaluation.Domain.Entities.Branch branchToCreate, CancellationToken token) =>
                {
                    branchToCreate.Id = Guid.NewGuid();
                    return branchToCreate;
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            _mockBranchRepository.Verify(r =>
                r.CreateAsync(It.Is<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(b => b.Name == command.Name), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = "Dado comando inválido, quando criar filial, então lança ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = CreateBranchCommandTestData.CreateInvalidCommand();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();

            _mockBranchRepository.Verify(r =>
                r.CreateAsync(It.IsAny<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
