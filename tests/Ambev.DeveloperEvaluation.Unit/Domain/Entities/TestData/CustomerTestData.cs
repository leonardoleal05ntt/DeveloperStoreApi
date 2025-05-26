using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using Bogus.Extensions.Brazil;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class CustomerTestData
    {
        private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
            .CustomInstantiator(f => new Customer(
                f.Person.FullName,
                f.Person.Cpf()
            ));

        public static Customer GenerateValidCustomer()
        {
            var command = CustomerFaker.Generate();
            command.Id = Guid.NewGuid();
            return command;
        }

        public static string GenerateValidCustomerName()
        {
            return new Faker().Person.FullName;
        }

        public static string GenerateValidDocumentNumber()
        {
            return new Faker().Person.Cpf(); 
        }

        public static string GenerateEmptyCustomerName()
        {
            return string.Empty;
        }

        public static string GenerateInvalidDocumentNumber()
        {
            return new Faker().Random.String2(5);
        }

        public static string GenerateLongCustomerName()
        {
            return new Faker().Random.String2(101);
        }
    }
}
