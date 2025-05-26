namespace Ambev.DeveloperEvaluation.Application.Sales.EditSale
{
    public class EditSaleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
