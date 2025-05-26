using Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch
{
    public static class InactiveBranchCommandTestData
    {
        private static readonly Faker _faker = new Faker();

        public static InactiveBranchCommand GenerateValidCommand()
        {
            return new InactiveBranchCommand { Id = _faker.Random.Guid() };
        }

        public static InactiveBranchCommand GenerateCommandWithEmptyId()
        {
            return new InactiveBranchCommand { Id = Guid.Empty };
        }
    }
}
