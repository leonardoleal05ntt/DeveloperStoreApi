using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductCommand : IRequest<PagedResult<ListProductResult>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public bool? Active { get; set; }

        public ListProductCommand(int pageNumber, int pageSize, bool? active, string? search)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Search = search;
            Active = active;
        }
    }
}
