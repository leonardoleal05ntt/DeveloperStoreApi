using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class ListSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<IMapper> _mockMapper;

        public ListSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact(DisplayName = "Dado comando válido, quando listar vendas, então retorna resultado paginado")]
        public async Task Handle_ValidRequest_ReturnsPagedResult()
        {
            // Arrange
            var command = ListSaleCommandTestData.Create();

            var sales = new List<Sale>
            {
                SaleTestData.GenerateValidSale(withItems: true),
                SaleTestData.GenerateValidSale(withItems: true)
            };

            var pagedSales = new PagedResult<Sale>
            {
                Items = sales,
                TotalCount = sales.Count
            };

            _mockSaleRepository
                .Setup(r => r.GetPagedAsync(
                    command.PageNumber,
                    command.PageSize,
                    command.Search,
                    command.Cancelled,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedSales);

            var mappedResults = new List<ListSaleResult>();

            foreach (var sale in sales)
            {
                mappedResults.Add(new ListSaleResult
                {
                    Id = sale.Id,
                    Date = sale.Date,
                    IsCancelled = sale.IsCancelled
                });
            }

            _mockMapper
                .Setup(m => m.Map<IEnumerable<ListSaleResult>>(sales))
                .Returns(mappedResults);

            var handler = new ListSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(sales.Count);
            result.TotalCount.Should().Be(sales.Count);

            _mockSaleRepository.Verify(r => r.GetPagedAsync(
                command.PageNumber,
                command.PageSize,
                command.Search,
                command.Cancelled,
                It.IsAny<CancellationToken>()), Times.Once);

            _mockMapper.Verify(m => m.Map<IEnumerable<ListSaleResult>>(sales), Times.Once);
        }

        [Fact(DisplayName = "Dado comando inválido, quando listar vendas, então lança ValidationException")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = ListSaleCommandTestData.CreateInvalid();

            var handler = new ListSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();

            _mockSaleRepository.Verify(r => r.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()), Times.Never);

            _mockMapper.Verify(m => m.Map<IEnumerable<ListSaleResult>>(It.IsAny<IEnumerable<Sale>>()), Times.Never);
        }
    }
}
