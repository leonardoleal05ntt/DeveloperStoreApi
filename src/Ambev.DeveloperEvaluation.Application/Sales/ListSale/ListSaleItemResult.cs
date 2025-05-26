namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleItemResult
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }  
        public decimal Discount { get; set; }   
        public decimal Total { get; set; }
    }
}
