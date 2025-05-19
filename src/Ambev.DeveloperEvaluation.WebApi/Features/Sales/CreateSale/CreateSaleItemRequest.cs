namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
