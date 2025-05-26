using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class BranchTests
    {
        [Fact(DisplayName = "Construtor da filial deve inicializar nome e status ativo")]
        public void Given_ValidName_When_CreatingBranch_Then_NameAndActiveShouldBeSet()
        {
            // Arrange
            var branchName = BranchTestData.GenerateValidBranchName();

            // Act
            var branch = new Branch(branchName);

            // Assert
            branch.Should().NotBeNull();
            branch.Name.Should().Be(branchName);
            branch.Active.Should().BeTrue();
        }


        [Fact(DisplayName = "Método Edit deve alterar o nome da filial")]
        public void Given_Branch_When_EditingName_Then_NameShouldBeUpdated()
        {
            // Arrange
            var branch = BranchTestData.GenerateValidBranch();
            var newName = BranchTestData.GenerateValidBranchName();
            newName.Should().NotBe(branch.Name); 

            // Act
            branch.Edit(newName);

            // Assert
            branch.Name.Should().Be(newName);
        }


        [Fact(DisplayName = "Método Inactive deve mudar o status de ativo para inativo")]
        public void Given_ActiveBranch_When_Inactivating_Then_StatusShouldBeInactive()
        {
            // Arrange
            var branch = BranchTestData.GenerateValidBranch(); 

            // Act
            branch.Inactive();

            // Assert
            branch.Active.Should().BeFalse();
        }
    }
}
