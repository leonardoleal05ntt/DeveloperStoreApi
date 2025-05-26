using Ambev.DeveloperEvaluation.Application.Branch.ListBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branch
{
    public static class ListBranchCommandTestData
    {
        private static readonly Faker _faker = new Faker();

        public static ListBranchCommand GenerateValidCommand()
        {
            return new ListBranchCommand(
                pageNumber: 1,
                pageSize: 10,
                active: null,
                search: null 
            );
        }

        public static ListBranchCommand GenerateValidCommandWithFilters()
        {
            return new ListBranchCommand(
                pageNumber: 2,
                pageSize: 5,
                active: true,
                search: _faker.Company.CompanyName() 
            );
        }

        public static ListBranchCommand GenerateCommandWithInvalidPageNumber()
        {
            return new ListBranchCommand(
                pageNumber: 0,
                pageSize: 10,
                active: null,
                search: null
            );
        }

        public static ListBranchCommand GenerateCommandWithInvalidPageSize()
        {
            return new ListBranchCommand(
                pageNumber: 1,
                pageSize: 0,
                active: null,
                search: null
            );
        }

        public static List<DeveloperEvaluation.Domain.Entities.Branch> GenerateBranchEntities(int count)
        {
            return new Faker<DeveloperEvaluation.Domain.Entities.Branch>()
                .CustomInstantiator(f => new DeveloperEvaluation.Domain.Entities.Branch(
                    f.Company.CompanyName()))
                .Generate(count);
        }

        public static List<ListBranchResult> GenerateListBranchResults(int count)
        {
            return new Faker<ListBranchResult>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Active, f => f.Random.Bool())
                .Generate(count);
        }
    }
}
