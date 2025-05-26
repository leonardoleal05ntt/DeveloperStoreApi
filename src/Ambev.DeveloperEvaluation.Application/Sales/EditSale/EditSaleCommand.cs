using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.EditSale
{
    public class EditSaleCommand : IRequest<Guid>
    {
        public string SaleNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<EditSaleItemDto> Items { get; set; } = new List<EditSaleItemDto>();
    }
}
