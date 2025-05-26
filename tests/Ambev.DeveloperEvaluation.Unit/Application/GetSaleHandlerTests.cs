using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.ComponentModel.Design;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = "Dado comando válido, quando buscar venda, então retorna resultado mapeado")]
        public async Task Handle_ValidRequest_ReturnsMappedResult()
        {
            // Arrange
            var command = new GetSaleCommand(Guid.NewGuid());

            var sale = SaleTestData.GenerateValidSale();
            sale.Id = command.Id;

            var expectedResult = new GetSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
            };

            _mockSaleRepository
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(m => m.Map<GetSaleResult>(sale))
                .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockSaleRepository.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<GetSaleResult>(sale), Times.Once);
        }

        [Fact(DisplayName = "Dado comando inválido, quando buscar venda, então lança ValidationException")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new GetSaleCommand(Guid.Empty);

            var handler = new GetSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            _mockSaleRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockMapper.Verify(m => m.Map<GetSaleResult>(It.IsAny<Sale>()), Times.Never);
        }

        [Fact(DisplayName = "Quando venda não existe, então lança KeyNotFoundException")]
        public async Task Handle_SaleDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = new GetSaleCommand(Guid.NewGuid());

            _mockSaleRepository
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {command.Id} not found");

            _mockSaleRepository.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<GetSaleResult>(It.IsAny<Sale>()), Times.Never);
        }
    }
}
