using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact(DisplayName = "Construtor de SaleItem deve inicializar propriedades e calcular desconto")]
        public void Given_ValidData_When_CreatingSaleItem_Then_PropertiesShouldBeSetAndDiscountCalculated()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 10.00m;

            // Act
            var saleItem = new SaleItem(productId, quantity, unitPrice);

            // Assert
            saleItem.Should().NotBeNull();
            saleItem.ProductId.Should().Be(productId);
            saleItem.Quantity.Should().Be(quantity);
            saleItem.UnitPrice.Should().Be(unitPrice);
            saleItem.Discount.Should().Be(0.10m);
            saleItem.Total.Should().Be(quantity * unitPrice * (1 - 0.10m));
        }

        [Theory(DisplayName = "Construtor de SaleItem deve aplicar o desconto correto com base na quantidade")]
        [InlineData(1, 0.0)]
        [InlineData(3, 0.0)]
        [InlineData(4, 0.10)]
        [InlineData(9, 0.10)]
        [InlineData(10, 0.20)]
        [InlineData(15, 0.20)]
        public void Given_Quantity_When_CreatingSaleItem_Then_CorrectDiscountShouldBeApplied(int quantity, decimal expectedDiscount)
        {
            // Arrange
            var productId = Guid.NewGuid();
            var unitPrice = 10.00m;

            // Act
            var saleItem = new SaleItem(productId, quantity, unitPrice);

            // Assert
            saleItem.Discount.Should().Be(expectedDiscount);
            saleItem.Total.Should().Be(quantity * unitPrice * (1 - expectedDiscount));
        }


        [Fact(DisplayName = "Método Update deve alterar quantidade e preço unitário, e recalcular o desconto")]
        public void Given_SaleItem_When_Updating_Then_PropertiesShouldBeUpdatedAndDiscountRecalculated()
        {
            // Arrange
            var saleItem = new SaleItem(Guid.NewGuid(), 1, 10.00m); 
            saleItem.Quantity.Should().Be(1);
            saleItem.Discount.Should().Be(0.0m);
            saleItem.Total.Should().Be(1 * 10.00m * (1 - 0.0m)); 

            var newQuantity = 5; 
            var newUnitPrice = 15.00m;

            // Act
            saleItem.Update(newQuantity, newUnitPrice);

            // Assert
            saleItem.Quantity.Should().Be(newQuantity);
            saleItem.UnitPrice.Should().Be(newUnitPrice);

            decimal expectedDiscountForNewQuantity;
            if (newQuantity >= 10) expectedDiscountForNewQuantity = 0.20m;
            else if (newQuantity >= 4) expectedDiscountForNewQuantity = 0.10m;
            else expectedDiscountForNewQuantity = 0.0m;

            saleItem.Discount.Should().Be(expectedDiscountForNewQuantity);
            saleItem.Total.Should().Be(newQuantity * newUnitPrice * (1 - expectedDiscountForNewQuantity));

            var saleItem2 = new SaleItem(Guid.NewGuid(), 2, 20.00m); 
            saleItem2.Update(12, 25.00m); 
            saleItem2.Quantity.Should().Be(12);
            saleItem2.UnitPrice.Should().Be(25.00m);
            saleItem2.Discount.Should().Be(0.20m);
            saleItem2.Total.Should().Be(12 * 25.00m * (1 - 0.20m)); 
        }
    }
}
