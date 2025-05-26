using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Construtor da venda deve inicializar propriedades e status de não cancelado")]
        public void Given_ValidData_When_CreatingSale_Then_PropertiesShouldBeSetAndNotCancelled()
        {
            // Arrange
            var saleNumber = SaleTestData.GenerateValidSaleNumber();
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();

            // Act
            var sale = new Sale(saleNumber, customerId, branchId);

            // Assert
            sale.Should().NotBeNull();
            sale.SaleNumber.Should().Be(saleNumber);
            sale.CustomerId.Should().Be(customerId);
            sale.BranchId.Should().Be(branchId);
            sale.IsCancelled.Should().BeFalse();
            sale.Items.Should().BeEmpty();
            sale.Total.Should().Be(0);
            sale.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact(DisplayName = "Método AddItem deve adicionar um item à lista de itens")]
        public void Given_Sale_When_AddingItem_Then_ItemShouldBeAddedAndTotalUpdated()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var item1 = SaleItemTestData.GenerateValidSaleItem();
            var item2 = SaleItemTestData.GenerateValidSaleItem();

            // Act
            sale.AddItem(item1);
            sale.AddItem(item2);

            // Assert
            sale.Items.Should().Contain(item1);
            sale.Items.Should().Contain(item2);
            sale.Items.Should().HaveCount(2);
            sale.Total.Should().Be(item1.Total + item2.Total);
        }

        [Fact(DisplayName = "Método RemoveItem deve remover um item da lista de itens")]
        public void Given_SaleWithItems_When_RemovingItem_Then_ItemShouldBeRemovedAndTotalUpdated()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var item1 = SaleItemTestData.GenerateValidSaleItem();
            var item2 = SaleItemTestData.GenerateValidSaleItem();
            sale.AddItem(item1);
            sale.AddItem(item2);
            sale.Items.Should().HaveCount(2);

            // Act
            sale.RemoveItem(item1);

            // Assert
            sale.Items.Should().NotContain(item1);
            sale.Items.Should().Contain(item2);
            sale.Items.Should().HaveCount(1);
            sale.Total.Should().Be(item2.Total);
        }

        [Fact(DisplayName = "Método Cancel deve mudar o status de não cancelado para cancelado")]
        public void Given_UncancelledSale_When_Cancelling_Then_StatusShouldBeCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.Cancel();

            // Assert
            sale.IsCancelled.Should().BeTrue();
        }

        [Fact(DisplayName = "Método Cancel deve mudar o status de cancelado para não cancelado")]
        public void Given_CancelledSale_When_Activating_Then_StatusShouldBeUncancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.Cancel();
            sale.IsCancelled.Should().BeTrue();

            // Act
            sale.Cancel();

            // Assert
            sale.IsCancelled.Should().BeFalse();
        }
    }
}
