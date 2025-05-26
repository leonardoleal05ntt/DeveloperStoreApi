using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class CustomerTests
    {
        [Fact(DisplayName = "Construtor do cliente deve inicializar nome, documento e status ativo")]
        public void Given_ValidData_When_CreatingCustomer_Then_PropertiesShouldBeSet()
        {
            // Arrange
            var customerName = CustomerTestData.GenerateValidCustomerName();
            var documentNumber = CustomerTestData.GenerateValidDocumentNumber();

            // Act
            var customer = new Customer(customerName, documentNumber);

            // Assert
            customer.Should().NotBeNull();
            customer.Name.Should().Be(customerName);
            customer.DocumentNumber.Should().Be(documentNumber);
            customer.Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Método Edit deve alterar nome e documento do cliente")]
        public void Given_Customer_When_Editing_Then_PropertiesShouldBeUpdated()
        {
            // Arrange
            var customer = CustomerTestData.GenerateValidCustomer();
            var newName = CustomerTestData.GenerateValidCustomerName();
            var newDocumentNumber = CustomerTestData.GenerateValidDocumentNumber();

            newName.Should().NotBe(customer.Name); 
            newDocumentNumber.Should().NotBe(customer.DocumentNumber); 

            // Act
            customer.Edit(newName, newDocumentNumber);

            // Assert
            customer.Name.Should().Be(newName);
            customer.DocumentNumber.Should().Be(newDocumentNumber);
        }

        [Fact(DisplayName = "Método Inactive deve mudar o status de ativo para inativo")]
        public void Given_ActiveCustomer_When_Inactivating_Then_StatusShouldBeInactive()
        {
            // Arrange
            var customer = CustomerTestData.GenerateValidCustomer(); 

            // Act
            customer.Inactive();

            // Assert
            customer.Active.Should().BeFalse();
        }

        [Fact(DisplayName = "Método Inactive deve mudar o status de inativo para ativo")]
        public void Given_InactiveCustomer_When_Activating_Then_StatusShouldBeActive()
        {
            // Arrange
            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Inactive(); 
            customer.Active.Should().BeFalse();

            // Act
            customer.Inactive(); 

            // Assert
            customer.Active.Should().BeTrue();
        }
    }
}
