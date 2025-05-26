using Ambev.DeveloperEvaluation.Application.Branch.EditBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch
{
    public static class EditBranchCommandTestData
    {
        private static readonly Faker<EditBranchCommand> _editBranchCommandFaker = new Faker<EditBranchCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Commerce.Department());

        public static EditBranchCommand GenerateValidCommand()
        {
            return _editBranchCommandFaker.Generate();
        }

        public static EditBranchCommand GenerateCommandWithEmptyId()
        {
            return _editBranchCommandFaker.Clone()
                .RuleFor(c => c.Id, Guid.Empty)
                .Generate();
        }

        public static EditBranchCommand GenerateCommandWithEmptyName()
        {
            return _editBranchCommandFaker.Clone()
                .RuleFor(c => c.Name, "")
                .Generate();
        }

        public static EditBranchCommand GenerateCommandWithSpecificId(Guid id)
        {
            return _editBranchCommandFaker.Clone()
                .RuleFor(c => c.Id, id)
                .Generate();
        }
    }
}
