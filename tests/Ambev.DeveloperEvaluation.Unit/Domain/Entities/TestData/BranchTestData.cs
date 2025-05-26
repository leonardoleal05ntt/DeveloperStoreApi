using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class BranchTestData
    {
        private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
            .CustomInstantiator(f => new Branch(f.Commerce.Department())); 

        public static Branch GenerateValidBranch()
        {
            var command = BranchFaker.Generate();
            command.Id = Guid.NewGuid();
            return command;
        }

        public static string GenerateValidBranchName()
        {
            return new Faker().Commerce.Department();
        }

        public static string GenerateEmptyBranchName()
        {
            return string.Empty;
        }

        public static string GenerateNullBranchName()
        {
            return null;
        }

        public static string GenerateLongBranchName()
        {
            return new Faker().Random.String2(101);
        }
    }
}
