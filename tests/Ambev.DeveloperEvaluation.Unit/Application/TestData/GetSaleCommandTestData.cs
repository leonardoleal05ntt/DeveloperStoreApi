using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class GetSaleCommandTestData
    {
        public static GetSaleCommand Create(Guid? id = null)
        {
            return new GetSaleCommand(id ?? Guid.NewGuid());
        }
    }
}
