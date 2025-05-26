namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSale { get; set; }
        public bool IsCancelled { get; set; }
        public List<GetSaleItemResult> Items { get; set; } = new List<GetSaleItemResult>();
    }
}
