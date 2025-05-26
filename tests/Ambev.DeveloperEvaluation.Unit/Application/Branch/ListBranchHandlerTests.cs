using Ambev.DeveloperEvaluation.Application.Branch.ListBranch;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branch
{
    public class ListBranchHandlerTests
    {
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ListBranchHandler _handler;

        public ListBranchHandlerTests()
        {
            _mockBranchRepository = new Mock<IBranchRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new ListBranchHandler(_mockBranchRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = "Dado comando válido, quando listar filiais, então retorna PagedResult com itens mapeados")]
        public async Task Handle_ValidCommand_ReturnsPagedResultWithMappedItems()
        {
            // Arrange
            var command = ListBranchCommandTestData.GenerateValidCommand();
            var branchEntities = ListBranchCommandTestData.GenerateBranchEntities(5); 
            var expectedTotalCount = 10;

            var pagedResultFromRepo = new PagedResult<Ambev.DeveloperEvaluation.Domain.Entities.Branch>
            {
                Items = branchEntities,
                TotalCount = expectedTotalCount
            };

            _mockBranchRepository.Setup(r => r.GetPagedAsync(
                                    It.Is<int>(pn => pn == command.PageNumber),
                                    It.Is<int>(ps => ps == command.PageSize),
                                    It.Is<string>(s => s == command.Search),
                                    It.Is<bool?>(a => a == command.Active),
                                    It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(pagedResultFromRepo);

            var expectedMappedResults = branchEntities.Select(b => new ListBranchResult { Id = b.Id, Name = b.Name, Active = b.Active }).ToList();
            _mockMapper.Setup(m => m.Map<IEnumerable<ListBranchResult>>(pagedResultFromRepo.Items))
                       .Returns(expectedMappedResults);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().NotBeNull().And.HaveCount(expectedMappedResults.Count);
            result.Items.Should().BeEquivalentTo(expectedMappedResults);
            result.TotalCount.Should().Be(expectedTotalCount);

            _mockBranchRepository.Verify(r => r.GetPagedAsync(
                                    It.Is<int>(pn => pn == command.PageNumber),
                                    It.Is<int>(ps => ps == command.PageSize),
                                    It.Is<string>(s => s == command.Search),
                                    It.Is<bool?>(a => a == command.Active),
                                    It.IsAny<CancellationToken>()),
                                    Times.Once);

            _mockMapper.Verify(m => m.Map<IEnumerable<ListBranchResult>>(pagedResultFromRepo.Items), Times.Once);
        }

        [Fact(DisplayName = "Dado comando com PageNumber inválido, quando listar filiais, então lança ValidationException")]
        public async Task Handle_InvalidPageNumber_ThrowsValidationException()
        {
            // Arrange
            var command = ListBranchCommandTestData.GenerateCommandWithInvalidPageNumber();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *'Page Number' must be greater than '0'.*");

            _mockBranchRepository.Verify(r => r.GetPagedAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockMapper.Verify(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()), Times.Never);
        }

        [Fact(DisplayName = "Dado comando com PageSize inválido, quando listar filiais, então lança ValidationException")]
        public async Task Handle_InvalidPageSize_ThrowsValidationException()
        {
            // Arrange
            var command = ListBranchCommandTestData.GenerateCommandWithInvalidPageSize();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *'Page Size' must be between 1 and 100. You entered 0.*");

            _mockBranchRepository.Verify(r => r.GetPagedAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockMapper.Verify(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()), Times.Never);
        }

        [Fact(DisplayName = "Dado comando válido e repositório retorna lista vazia, quando listar filiais, então retorna PagedResult vazio")]
        public async Task Handle_RepositoryReturnsEmptyList_ReturnsEmptyPagedResult()
        {
            // Arrange
            var command = ListBranchCommandTestData.GenerateValidCommand();

            var pagedResultFromRepo = new PagedResult<Ambev.DeveloperEvaluation.Domain.Entities.Branch>
            {
                Items = new List<Ambev.DeveloperEvaluation.Domain.Entities.Branch>(),
                TotalCount = 0
            };

            _mockBranchRepository.Setup(r => r.GetPagedAsync(
                                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(pagedResultFromRepo);

            _mockMapper.Setup(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()))
                       .Returns(new List<ListBranchResult>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);

            _mockBranchRepository.Verify(r => r.GetPagedAsync(
                                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()),
                                    Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()), Times.Once);
        }

        [Fact(DisplayName = "Dado comando com filtros de busca e ativo, quando listar filiais, então passa filtros para o repositório")]
        public async Task Handle_CommandWithFilters_PassesFiltersToRepository()
        {
            // Arrange
            var command = ListBranchCommandTestData.GenerateValidCommandWithFilters();
            var branchEntities = ListBranchCommandTestData.GenerateBranchEntities(2);
            var pagedResultFromRepo = new PagedResult<Ambev.DeveloperEvaluation.Domain.Entities.Branch> { Items = branchEntities, TotalCount = 5 };

            _mockBranchRepository.Setup(r => r.GetPagedAsync(
                                    It.Is<int>(pn => pn == command.PageNumber),
                                    It.Is<int>(ps => ps == command.PageSize),
                                    It.Is<string>(s => s == command.Search),
                                    It.Is<bool?>(a => a == command.Active),
                                    It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(pagedResultFromRepo);

            _mockMapper.Setup(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()))
                       .Returns(ListBranchCommandTestData.GenerateListBranchResults(2));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockBranchRepository.Verify(r => r.GetPagedAsync(
                                    command.PageNumber,
                                    command.PageSize,
                                    command.Search,
                                    command.Active,
                                    It.IsAny<CancellationToken>()),
                                    Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ListBranchResult>>(It.IsAny<IEnumerable<Ambev.DeveloperEvaluation.Domain.Entities.Branch>>()), Times.Once);
        }
    }
}
