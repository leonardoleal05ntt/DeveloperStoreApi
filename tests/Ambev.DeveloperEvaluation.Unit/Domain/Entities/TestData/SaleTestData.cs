using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                f.Random.AlphaNumeric(10).ToUpper(), 
                CustomerTestData.GenerateValidCustomer().Id, 
                BranchTestData.GenerateValidBranch().Id 
            ))
            .RuleFor(s => s.Date, f => f.Date.Recent(30)) 
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool()); 

        public static Sale GenerateValidSale(bool withItems = false, int itemCount = 3)
        {
            var sale = SaleFaker.Generate();
            sale.Id = Guid.NewGuid();
            if (withItems)
            {
                var items = SaleItemTestData.GenerateValidSaleItems(itemCount);
                foreach (var item in items)
                {
                    item.Id = Guid.NewGuid();
                    sale.AddItem(item);
                }
            }
            return sale;
        }

        public static string GenerateValidSaleNumber()
        {
            return new Faker().Random.AlphaNumeric(10).ToUpper();
        }

        public static string GenerateEmptySaleNumber()
        {
            return string.Empty;
        }
    }
}
