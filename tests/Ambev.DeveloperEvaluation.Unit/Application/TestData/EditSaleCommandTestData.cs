using Ambev.DeveloperEvaluation.Application.Sales.EditSale;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class EditSaleCommandTestData
    {
        private static readonly Faker<EditSaleItemDto> EditSaleItemDtoFaker = new Faker<EditSaleItemDto>()
        .RuleFor(i => i.ProductId, f => ProductTestData.GenerateValidProduct().Id)
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 15))
        .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(5, 200));

        private static readonly Faker<EditSaleCommand> EditSaleCommandFaker = new Faker<EditSaleCommand>()
            .RuleFor(c => c.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(c => c.CustomerId, f => CustomerTestData.GenerateValidCustomer().Id)
            .RuleFor(c => c.BranchId, f => BranchTestData.GenerateValidBranch().Id)
            .RuleFor(c => c.Items, f => EditSaleItemDtoFaker.Generate(f.Random.Number(1, 5)).ToList());

        public static EditSaleCommand GenerateValidCommand()
        {
            var command = EditSaleCommandFaker.Generate();
            command.CustomerId = Guid.NewGuid();
            command.BranchId = Guid.NewGuid();
            return command;
        }

        public static EditSaleCommand GenerateCommandWithEmptySaleNumber()
        {
            var command = EditSaleCommandFaker.Generate();
            command.SaleNumber = string.Empty;
            return command;
        }

        public static EditSaleCommand GenerateCommandWithEmptyCustomerId()
        {
            var command = EditSaleCommandFaker.Generate();
            command.CustomerId = Guid.Empty;
            return command;
        }

        public static EditSaleCommand GenerateCommandWithEmptyBranchId()
        {
            var command = EditSaleCommandFaker.Generate();
            command.BranchId = Guid.Empty;
            return command;
        }

        public static EditSaleCommand GenerateCommandWithNoItems()
        {
            var command = EditSaleCommandFaker.Generate();
            command.Items = new List<EditSaleItemDto>();
            return command;
        }

        public static EditSaleCommand GenerateCommandWithInvalidProductIdInItem()
        {
            var command = EditSaleCommandFaker.Generate();
            command.CustomerId = Guid.NewGuid();
            command.BranchId = Guid.NewGuid();
            command.Items.First().ProductId = Guid.NewGuid();
            return command;
        }
    }
}
