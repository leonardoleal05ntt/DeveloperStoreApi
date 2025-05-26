using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.InactiveProduct
{
    public class InactiveProductCommandHandler : IRequestHandler<InactiveProductCommand>
    {
        private readonly IProductRepository _productRepository;
        public InactiveProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(InactiveProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new InactiveProductValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with id {command.Id} does not exist");

            existingProduct.Inactive();
            await _productRepository.UpdateAsync(existingProduct);
        }
    }
}
