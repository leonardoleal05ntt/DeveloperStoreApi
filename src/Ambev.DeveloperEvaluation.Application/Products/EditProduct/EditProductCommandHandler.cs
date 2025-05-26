using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.EditProduct
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
    {
        private readonly IProductRepository _productRepository;
        public EditProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(EditProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new EditProductValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with id {command.Id} does not exist");

            existingProduct.Edit(command.Name);
            await _productRepository.UpdateAsync(existingProduct);
        }
    }
}
