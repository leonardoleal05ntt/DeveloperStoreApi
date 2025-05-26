using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class ProductTestData
    {
        private static readonly Faker<Product> ProductFaker = new Faker<Product>()
            .CustomInstantiator(f => new Product(f.Commerce.ProductName()));

        public static Product GenerateValidProduct()
        {
            var command = ProductFaker.Generate();
            command.Id = Guid.NewGuid();
            return command;
        }

        public static string GenerateValidProductName()
        {
            return new Faker().Commerce.ProductName();
        }

        public static string GenerateEmptyProductName()
        {
            return string.Empty;
        }

        public static string GenerateLongProductName()
        {
            return new Faker().Random.String2(101); 
        }
    }
}
