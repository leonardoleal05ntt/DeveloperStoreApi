﻿using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.EditSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.EditSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing user operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The user creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = response
            });
        }

        /// <summary>
        /// Edit a new sale
        /// </summary>
        /// <param name="request">The user creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The edit sale details</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditSale([FromBody] EditSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new EditSaleRequestValidatior();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<EditSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Sale edited successfully",
                Data = response
            });
        }

        /// <summary>
        /// Retrieves a paginated list of sales
        /// </summary>
        /// <param name="request">Pagination and search parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged list of sales</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<PagedResult<ListSaleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedSales([FromQuery] ListSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ListSaleCommand>(request);
            var pagedResult = await _mediator.Send(command, cancellationToken);

            var response = new PagedResult<ListSaleResponse>
            {
                Items = _mapper.Map<IEnumerable<ListSaleResponse>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount
            };

            return Ok(new ApiResponseWithData<PagedResult<ListSaleResponse>>
            {
                Success = true,
                Message = "Sales retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Cancel a sale by your ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InactiveSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new CancelSaleRequest { Id = id };
            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CancelSaleCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Sale Cancelled successfully"
            });
        }

        /// <summary>
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetSaleCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<GetSaleResponse>
            {
                Success = true,
                Message = "Sale retrieved successfully",
                Data = _mapper.Map<GetSaleResponse>(response)
            });
        }
    }
}
