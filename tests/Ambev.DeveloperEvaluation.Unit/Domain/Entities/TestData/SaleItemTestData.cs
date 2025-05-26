using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleItemTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .CustomInstantiator(f =>
            {
                var productId = ProductTestData.GenerateValidProduct().Id;
                var quantity = f.Random.Number(1, 20);
                var unitPrice = f.Finance.Amount(1, 100);

                return new SaleItem(productId, quantity, unitPrice);
            });

        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }

        public static List<SaleItem> GenerateValidSaleItems(int count = 3)
        {
            return SaleItemFaker.Generate(count);
        }
    }
}
