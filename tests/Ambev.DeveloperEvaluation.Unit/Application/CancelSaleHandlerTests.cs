using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly CancelSaleCommandHandler _handler;

        public CancelSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _handler = new CancelSaleCommandHandler(_mockSaleRepository.Object);
        }

        [Fact(DisplayName = "Dado comando válido, quando cancelar venda, então atualiza venda")]
        public async Task Handle_ValidCommand_UpdatesSale()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { Id = saleId };

            var existingSale = SaleTestData.GenerateValidSale();
            existingSale.Id = saleId;

            _mockSaleRepository
                .Setup(r => r.GetByIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            _mockSaleRepository
                .Setup(r => r.UpdateAsync(existingSale, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSaleRepository.Verify(r => r.GetByIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _mockSaleRepository.Verify(r => r.UpdateAsync(existingSale, It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(existingSale.IsCancelled);
        }

        [Fact(DisplayName = "Dado comando inválido, quando cancelar venda, então lança ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CancelSaleCommand { Id = Guid.Empty };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Quando venda não existe, então lança InvalidOperationException")]
        public async Task Handle_SaleDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { Id = saleId };

            _mockSaleRepository
                .Setup(r => r.GetByIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with id {saleId} does not exist");
        }
    }
}
