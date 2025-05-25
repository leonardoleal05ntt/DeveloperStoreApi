using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.InactiveProduct
{
    public class InactiveProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
