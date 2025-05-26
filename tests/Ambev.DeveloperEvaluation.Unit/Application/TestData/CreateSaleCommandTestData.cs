using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleCommandTestData
    {
        private static readonly Faker<CreateSaleItemDto> CreateSaleItemDtoFaker = new Faker<CreateSaleItemDto>()
            .RuleFor(i => i.ProductId, f => ProductTestData.GenerateValidProduct().Id)
            .RuleFor(i => i.Quantity, f => f.Random.Number(1, 15))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(5, 200));

        private static readonly Faker<CreateSaleCommand> CreateSaleCommandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(c => c.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(c => c.CustomerId, f => CustomerTestData.GenerateValidCustomer().Id)
            .RuleFor(c => c.BranchId, f => BranchTestData.GenerateValidBranch().Id)
            .RuleFor(c => c.Items, f => CreateSaleItemDtoFaker.Generate(f.Random.Number(1, 5)).ToList());

        public static CreateSaleCommand GenerateValidCommand()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.BranchId = Guid.NewGuid();
            command.CustomerId = Guid.NewGuid();
            return command;
        }

        public static CreateSaleCommand GenerateCommandWithEmptySaleNumber()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.SaleNumber = string.Empty;
            return command;
        }

        public static CreateSaleCommand GenerateCommandWithEmptyCustomerId()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.CustomerId = Guid.Empty;
            return command;
        }

        public static CreateSaleCommand GenerateCommandWithEmptyBranchId()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.BranchId = Guid.Empty;
            return command;
        }

        public static CreateSaleCommand GenerateCommandWithNoItems()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.Items = new List<CreateSaleItemDto>();
            return command;
        }

        public static CreateSaleCommand GenerateCommandWithInvalidProductIdInItem()
        {
            var command = CreateSaleCommandFaker.Generate();
            command.BranchId = Guid.NewGuid();
            command.CustomerId = Guid.NewGuid();
            command.Items.First().ProductId = Guid.NewGuid();
            return command;
        }
    }
}
