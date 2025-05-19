using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.EditSale
{
    public class EditSaleCommandHandler : IRequestHandler<EditSaleCommand, Guid>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        public readonly IProductRepository _productRepository;
        public EditSaleCommandHandler(ISaleRepository saleRepository, ICustomerRepository customerRepository, IBranchRepository branchRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(EditSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new EditSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (existingSale == null)
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} does not exist");

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
                throw new InvalidOperationException($"Customer with Id {command.CustomerId} does not exist");

            var branch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {command.BranchId} does not exist");

            var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();
            var existingProducts = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
            var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();

            foreach (var item in command.Items)
            {
                if (!existingProductIds.Contains(item.ProductId))
                    throw new InvalidOperationException($"Product with Id {item.ProductId} does not exist");
            }

            var updatedProductIds = new HashSet<Guid>();
            foreach (var item in command.Items)
            {
                var existingItem = existingSale.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (existingItem != null)
                    existingItem.Update(item.Quantity, item.UnitPrice); 
                else
                {
                    var newItem = new SaleItem(item.ProductId, item.Quantity, item.UnitPrice);
                    existingSale.AddItem(newItem);
                }

                updatedProductIds.Add(item.ProductId);
            }

            var itemsToRemove = existingSale.Items
                .Where(i => !updatedProductIds.Contains(i.ProductId))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                existingSale.RemoveItem(item);
            }

            await _saleRepository.UpdateAsync(existingSale);
            return existingSale.Id;
        }
    }
}
