using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleItemRequest> Items { get; set; }
    }
}
