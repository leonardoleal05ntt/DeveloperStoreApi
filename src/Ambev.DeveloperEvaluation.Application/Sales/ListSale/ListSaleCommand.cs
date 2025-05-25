using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleCommand : IRequest<PagedResult<ListSaleResult>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public bool? Cancelled { get; set; }

        public ListSaleCommand(int pageNumber, int pageSize, bool? cancelled, string? search)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Search = search;
            Cancelled = cancelled;
        }
    }
}
