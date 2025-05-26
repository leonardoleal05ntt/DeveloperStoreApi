using Ambev.DeveloperEvaluation.Application.Sales.ListSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class ListSaleCommandTestData
    {
        public static ListSaleCommand Create(
        int pageNumber = 1,
        int pageSize = 10,
        string search = "",
        bool? cancelled = null)
        {
            return new ListSaleCommand(pageNumber, pageSize, cancelled, search);
        }

        public static ListSaleCommand CreateInvalid()
        {
            return new ListSaleCommand(0, 0);
        }
    }
}
