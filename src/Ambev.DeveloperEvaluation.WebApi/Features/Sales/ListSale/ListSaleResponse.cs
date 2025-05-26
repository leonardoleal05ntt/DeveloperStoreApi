namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale
{
    public class ListSaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSale { get; set; }
        public bool IsCancelled { get; set; }
        public List<ListSaleItemResponse> Items { get; set; } = new List<ListSaleItemResponse>();
    }
}
