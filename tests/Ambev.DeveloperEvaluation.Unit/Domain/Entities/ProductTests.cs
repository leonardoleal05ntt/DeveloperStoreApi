using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class ProductTests
    {
        [Fact(DisplayName = "Construtor do produto deve inicializar nome e status ativo")]
        public void Given_ValidName_When_CreatingProduct_Then_NameAndActiveShouldBeSet()
        {
            // Arrange
            var productName = ProductTestData.GenerateValidProductName();

            // Act
            var product = new Product(productName);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(productName);
            product.Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Método Edit deve alterar o nome do produto")]
        public void Given_Product_When_EditingName_Then_NameShouldBeUpdated()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();
            var newName = ProductTestData.GenerateValidProductName();
            newName.Should().NotBe(product.Name); 

            // Act
            product.Edit(newName);

            // Assert
            product.Name.Should().Be(newName);
        }


        [Fact(DisplayName = "Método Inactive deve mudar o status de ativo para inativo")]
        public void Given_ActiveProduct_When_Inactivating_Then_StatusShouldBeInactive()
        {
            // Arrange
            var product = ProductTestData.GenerateValidProduct();

            // Act
            product.Inactive();

            // Assert
            product.Active.Should().BeFalse();
        }
    }
}
