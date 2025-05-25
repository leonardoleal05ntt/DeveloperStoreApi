using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers
{
    public class ListCustomerHandler : IRequestHandler<ListCustomerCommand, PagedResult<ListCustomerResult>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public ListCustomerHandler(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ListCustomerResult>> Handle(ListCustomerCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListCustomerValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var pagedResult = await _customerRepository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.Active, cancellationToken);

            var dtoItems = _mapper.Map<IEnumerable<ListCustomerResult>>(pagedResult.Items);

            return new PagedResult<ListCustomerResult>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}
