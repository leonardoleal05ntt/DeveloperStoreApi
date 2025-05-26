using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;
        public CancelSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingSale == null)
                throw new InvalidOperationException($"Sale with id {command.Id} does not exist");

            existingSale.Cancel();
            await _saleRepository.UpdateAsync(existingSale);
        }
    }
}
