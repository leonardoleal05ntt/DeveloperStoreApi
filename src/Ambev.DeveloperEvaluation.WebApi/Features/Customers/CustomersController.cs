using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.EditCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.InactiveCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.EditCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.InactiveCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomer;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    /// <summary>
    /// Controller for managing customer operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="request">The customer creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created customer details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateCustomerCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Customer created successfully",
                Data = response
            });
        }

        /// <summary>
        /// Retrieves a customer by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetCustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetCustomerRequest { Id = id };
            var validator = new GetCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetCustomerCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<GetCustomerResponse>
            {
                Success = true,
                Message = "Customer retrieved successfully",
                Data = _mapper.Map<GetCustomerResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves a paginated list of customers
        /// </summary>
        /// <param name="request">Pagination and search parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged list of customers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<PagedResult<ListCustomerResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedCustomers([FromQuery] ListCustomerRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ListCustomerCommand>(request);
            var pagedResult = await _mediator.Send(command, cancellationToken);

            var response = new PagedResult<ListCustomerResponse>
            {
                Items = _mapper.Map<IEnumerable<ListCustomerResponse>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount
            };

            return Ok(new ApiResponseWithData<PagedResult<ListCustomerResponse>>
            {
                Success = true,
                Message = "Customers retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Inactivates a customer by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer details if found</returns>
        [HttpPut("{id}/inactive")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InactiveCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new InactiveCustomerRequest { Id = id };
            var validator = new InactiveCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<InactiveCustomerCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Customer inactivated successfully"
            });
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="request">The customer update request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated customer details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] EditCustomerRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
                return BadRequest("The ID in the route does not match the ID in the request body.");

            var validator = new EditCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<EditCustomerCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Customer updated successfully"
            });
        }

    }
}
