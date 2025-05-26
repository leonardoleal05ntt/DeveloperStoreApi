using Ambev.DeveloperEvaluation.Application.Branch.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branch
{
    public class GetBranchHandlerTests
    {
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetBranchHandler _handler;

        public GetBranchHandlerTests()
        {
            _mockBranchRepository = new Mock<IBranchRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetBranchHandler(_mockBranchRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = "Dado comando válido e filial existente, quando buscar filial, então retorna GetBranchResult")]
        public async Task Handle_ValidCommandAndBranchExists_ReturnsGetBranchResult()
        {
            // Arrange
            var command = GetBranchCommandTestData.GenerateValidCommand();

            // Simula a entidade Branch que o repositório retornaria
            var branchFromRepo = new DeveloperEvaluation.Domain.Entities.Branch("Minha Filial SP");

            var expectedResult = new GetBranchResult { Id = command.Id, Name = "Minha Filial SP", Active = true };

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(branchFromRepo);

            _mockMapper.Setup(m => m.Map<GetBranchResult>(branchFromRepo))
                       .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);

            _mockBranchRepository.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);

            _mockMapper.Verify(m => m.Map<GetBranchResult>(branchFromRepo), Times.Once);
        }

        [Fact(DisplayName = "Dado comando com ID vazio, quando buscar filial, então lança ValidationException")]
        public async Task Handle_InvalidCommand_EmptyId_ThrowsValidationException()
        {
            // Arrange
            var command = GetBranchCommandTestData.GenerateCommandWithEmptyId();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *User ID is required*");

            _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetBranchResult>(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>()), Times.Never);
        }

        [Fact(DisplayName = "Dado filial inexistente, quando buscar filial, então lança KeyNotFoundException")]
        public async Task Handle_BranchDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = GetBranchCommandTestData.GenerateValidCommand();

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Branch)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Branch with ID {command.Id} not found");

            _mockMapper.Verify(m => m.Map<GetBranchResult>(It.IsAny<DeveloperEvaluation.Domain.Entities.Branch>()), Times.Never);
        }
    }
}
