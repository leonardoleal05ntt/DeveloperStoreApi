namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.EditSale
{
    public class EditSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
