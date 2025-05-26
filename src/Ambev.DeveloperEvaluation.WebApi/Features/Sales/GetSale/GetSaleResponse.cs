namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSale { get; set; }
        public bool IsCancelled { get; set; }
        public List<GetSaleItemResponse> Items { get; set; } = new List<GetSaleItemResponse>();
    }
}
