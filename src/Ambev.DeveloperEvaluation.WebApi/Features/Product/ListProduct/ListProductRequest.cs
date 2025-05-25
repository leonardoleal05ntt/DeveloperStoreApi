namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.ListProduct
{
    public class ListProductRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool? Active { get; set; }
    }
}
