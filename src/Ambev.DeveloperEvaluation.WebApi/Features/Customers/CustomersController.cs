﻿using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    /// <summary>
    /// Controller for managing user operations
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
        /// <param name="request">The user creation request</param>
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
    }
}
