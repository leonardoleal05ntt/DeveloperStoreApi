using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Guid>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        public readonly IProductRepository _productRepository;
        public CreateSaleCommandHandler(ISaleRepository saleRepository, ICustomerRepository customerRepository, IBranchRepository branchRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCustomer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (existingCustomer == null)
                throw new InvalidOperationException($"Customer with Id {command.CustomerId} does not exist");

            var existingBranch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
            if (existingBranch == null)
                throw new InvalidOperationException($"Branch with Id {command.BranchId} does not exist");

            var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (existingSale != null)
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");

            var sale = new Sale(command.SaleNumber, command.CustomerId, command.BranchId);

            var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();
            var existingProducts = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

            var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();

            foreach (var item in command.Items)
            {
                if (!existingProductIds.Contains(item.ProductId))
                    throw new InvalidOperationException($"Product with Id {item.ProductId} does not exist");
                
                var saleItem = new SaleItem(item.ProductId, item.Quantity, item.UnitPrice);
                sale.AddItem(saleItem);
            }

            await _saleRepository.CreateAsync(sale);
            return sale.Id;
        }
    }
}
