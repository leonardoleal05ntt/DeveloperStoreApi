using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleHandler : IRequestHandler<ListSaleCommand, PagedResult<ListSaleResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public ListSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListSaleResult>> Handle(ListSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var pagedResult = await _saleRepository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.Cancelled, cancellationToken);

            var dtoItems = _mapper.Map<IEnumerable<ListSaleResult>>(pagedResult.Items);

            return new PagedResult<ListSaleResult>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}
