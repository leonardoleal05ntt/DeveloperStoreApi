namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.EditSale
{
    public class EditSaleRequest
    {
        public string SaleNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<EditSaleItemRequest> Items { get; set; } = new List<EditSaleItemRequest>();
    }
}
