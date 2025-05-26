using Ambev.DeveloperEvaluation.Application.Sales.EditSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class EditSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly EditSaleCommandHandler _handler;

        public EditSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockBranchRepository = new Mock<IBranchRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _handler = new EditSaleCommandHandler(
                _mockSaleRepository.Object,
                _mockCustomerRepository.Object,
                _mockBranchRepository.Object,
                _mockProductRepository.Object
            );
        }

        [Fact(DisplayName = "Dado dados de venda válidos, quando editar venda, então retorna o ID da venda")]
        public async Task Handle_ValidRequest_ReturnsSaleId()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateValidCommand();

            var existingSale = SaleTestData.GenerateValidSale();
            existingSale.Update(command.SaleNumber, command.CustomerId, command.BranchId);
            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);

            var existingProducts = command.Items.Select(i =>
            {
                var product = ProductTestData.GenerateValidProduct();
                product.Id = i.ProductId;
                return product;
            }).ToList();

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProducts);

            // Act
            var saleId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            saleId.Should().Be(existingSale.Id);
            _mockSaleRepository.Verify(r => r.UpdateAsync(existingSale, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Dado dados inválidos, quando editar venda, então lança ValidationException")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateCommandWithEmptySaleNumber();

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *");
        }

        [Fact(DisplayName = "Quando venda não existe, então lança InvalidOperationException")]
        public async Task Handle_SaleDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateValidCommand();

            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with number {command.SaleNumber} does not exist");
        }

        [Fact(DisplayName = "Quando cliente não existe, então lança InvalidOperationException")]
        public async Task Handle_CustomerDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateValidCommand();

            var existingSale = SaleTestData.GenerateValidSale();
            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Customer with Id {command.CustomerId} does not exist");
        }

        [Fact(DisplayName = "Quando filial não existe, então lança InvalidOperationException")]
        public async Task Handle_BranchDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateValidCommand();

            var existingSale = SaleTestData.GenerateValidSale();
            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            var customer = CustomerTestData.GenerateValidCustomer();
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Branch)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Branch with Id {command.BranchId} does not exist");
        }

        [Fact(DisplayName = "Quando produto de item não existe, então lança InvalidOperationException")]
        public async Task Handle_ProductDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateCommandWithInvalidProductIdInItem();

            var existingSale = SaleTestData.GenerateValidSale();
            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            var customer = CustomerTestData.GenerateValidCustomer();
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var branch = BranchTestData.GenerateValidBranch();
            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);

            var existingProducts = command.Items
                .Where(i => i.ProductId != command.Items.First().ProductId)
                .Select(i =>
                {
                    var product = ProductTestData.GenerateValidProduct();
                    product.Id = i.ProductId;
                    return product;
                }).ToList();

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProducts);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Product with Id {command.Items.First().ProductId} does not exist");
        }

        [Fact(DisplayName = "Quando editar venda, então chama UpdateAsync no repositório com a entidade atualizada")]
        public async Task Handle_ValidRequest_CallsUpdateAsyncWithUpdatedSaleEntity()
        {
            // Arrange
            var command = EditSaleHandlerTestData.GenerateValidCommand();

            var existingSale = SaleTestData.GenerateValidSale();
            existingSale.Update(command.SaleNumber, command.CustomerId, command.BranchId);
            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);

            var existingProducts = command.Items.Select(i =>
            {
                var product = ProductTestData.GenerateValidProduct();
                product.Id = i.ProductId;
                return product;
            }).ToList();

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProducts);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSaleRepository.Verify(r => r.UpdateAsync(existingSale, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
