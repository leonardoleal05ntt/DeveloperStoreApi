namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSale { get; set; }
        public List<ListSaleItemResult> Items { get; set; } = new List<ListSaleItemResult>();
    }
}
