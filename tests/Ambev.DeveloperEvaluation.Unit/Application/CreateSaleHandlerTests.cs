using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockBranchRepository = new Mock<IBranchRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _handler = new CreateSaleCommandHandler(
                _mockSaleRepository.Object,
                _mockCustomerRepository.Object,
                _mockBranchRepository.Object,
                _mockProductRepository.Object
            );
        }

        [Fact(DisplayName = "Dado dados de venda válidos, quando criar venda, então retorna o ID da venda")]
        public async Task Handle_ValidRequest_ReturnsSaleId()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);

            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale)null);

            var existingProducts = new List<Product>();
            foreach (var itemDto in command.Items)
            {
                var product = ProductTestData.GenerateValidProduct();
                product.Id = itemDto.ProductId;
                existingProducts.Add(product);
            }

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProducts);

            // Act
            var saleId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockCustomerRepository.Verify(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockBranchRepository.Verify(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()), Times.Once);
            _mockSaleRepository.Verify(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockProductRepository.Verify(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Dado dados de venda inválidos, quando criar venda, então lança ValidationException")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateCommandWithEmptySaleNumber();

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: *");
        }

        [Fact(DisplayName = "Quando cliente não existe, então lança InvalidOperationException")]
        public async Task Handle_CustomerDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            _mockCustomerRepository.Setup(r => r.GetByIdAsync(command.CustomerId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync((Customer)null);

            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.BranchId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(branch);
            _mockSaleRepository.Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync((Sale)null);

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
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository.Setup(r => r.GetByIdAsync(command.CustomerId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(customer);

            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.BranchId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Branch)null);

            _mockSaleRepository.Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync((Sale)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Branch with Id {command.BranchId} does not exist");
        }

        [Fact(DisplayName = "Quando número de venda já existe, então lança InvalidOperationException")]
        public async Task Handle_SaleNumberAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository.Setup(r => r.GetByIdAsync(command.CustomerId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(customer);
            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.BranchId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(branch);

            var existingSale = SaleTestData.GenerateValidSale();
            existingSale.Update(command.SaleNumber, command.CustomerId, command.BranchId);
            _mockSaleRepository.Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(existingSale);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with number {command.SaleNumber} already exists");
        }

        [Fact(DisplayName = "Quando um produto em item de venda não existe, então lança InvalidOperationException")]
        public async Task Handle_ProductInItemDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateCommandWithInvalidProductIdInItem();

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository.Setup(r => r.GetByIdAsync(command.CustomerId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(customer);
            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository.Setup(r => r.GetByIdAsync(command.BranchId, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(branch);
            _mockSaleRepository.Setup(r => r.GetBySaleNumberAsync(command.SaleNumber, It.Is<CancellationToken>(c => true)))
                .ReturnsAsync((Sale)null);

            var existingProducts = new List<Product>();
            foreach (var itemDto in command.Items.Where(i => i.ProductId != command.Items.First().ProductId))
            {
                var product = ProductTestData.GenerateValidProduct();
                product.Id = itemDto.ProductId;
                existingProducts.Add(product);
            }
            _mockProductRepository.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.Is<CancellationToken>(c => true)))
                .ReturnsAsync(existingProducts);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Product with Id {command.Items.First().ProductId} does not exist");
        }

        [Fact(DisplayName = "Quando cria venda, então chama CreateAsync no repositório com a entidade correta")]
        public async Task Handle_ValidRequest_CallsCreateAsyncWithCorrectSaleEntity()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Id = command.CustomerId;
            _mockCustomerRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var branch = BranchTestData.GenerateValidBranch();
            branch.Id = command.BranchId;
            _mockBranchRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);

            _mockSaleRepository
                .Setup(r => r.GetBySaleNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale)null);

            var existingProducts = new List<Product>();
            foreach (var itemDto in command.Items)
            {
                var product = ProductTestData.GenerateValidProduct();
                product.Id = itemDto.ProductId;
                existingProducts.Add(product);
            }

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProducts);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            _mockCustomerRepository.Verify(r => r.GetByIdAsync(command.CustomerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockBranchRepository.Verify(r => r.GetByIdAsync(command.BranchId, It.IsAny<CancellationToken>()), Times.Once);
            _mockSaleRepository.Verify(r => r.GetBySaleNumberAsync(command.SaleNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockProductRepository.Verify(r => r.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
