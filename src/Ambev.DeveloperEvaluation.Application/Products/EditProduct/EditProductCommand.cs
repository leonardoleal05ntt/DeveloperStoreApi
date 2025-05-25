using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.EditProduct
{
    public class EditProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
