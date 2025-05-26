using Ambev.DeveloperEvaluation.Application.Branch.GetBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch
{
    public static class GetBranchCommandTestData
    {
        private static readonly Faker _faker = new Faker();

        public static GetBranchCommand GenerateValidCommand()
        {
            return new GetBranchCommand(_faker.Random.Guid());
        }

        public static GetBranchCommand GenerateCommandWithEmptyId()
        {
            return new GetBranchCommand(Guid.Empty);
        }
    }
}
