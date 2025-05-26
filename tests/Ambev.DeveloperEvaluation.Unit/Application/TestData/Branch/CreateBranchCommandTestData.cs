using Ambev.DeveloperEvaluation.Application.Branch.CreateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch
{
    public static class CreateBranchCommandTestData
    {
        private static readonly Faker<CreateBranchCommand> CreateBranchFaker = new Faker<CreateBranchCommand>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName());

        public static CreateBranchCommand CreateValidCommand()
        {
            return CreateBranchFaker.Generate();
        }

        public static CreateBranchCommand CreateInvalidCommand()
        {
            return new CreateBranchCommand
            {
                Name = ""
            };
        }
    }
}
