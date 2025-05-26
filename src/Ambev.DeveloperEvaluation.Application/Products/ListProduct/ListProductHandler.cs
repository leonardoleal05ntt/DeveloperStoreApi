using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductHandler : IRequestHandler<ListProductCommand, PagedResult<ListProductResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ListProductHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListProductResult>> Handle(ListProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListProductValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var pagedResult = await _productRepository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.Active, cancellationToken);

            var dtoItems = _mapper.Map<IEnumerable<ListProductResult>>(pagedResult.Items);

            return new PagedResult<ListProductResult>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}
