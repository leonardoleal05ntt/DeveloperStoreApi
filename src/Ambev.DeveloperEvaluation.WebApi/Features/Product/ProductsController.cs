using Ambev.DeveloperEvaluation.Application.Customers.EditCustomer;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.EditProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.EditCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.EditProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product
{
    /// <summary>
    /// Controller for managing product operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="request">The product creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created product details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateProductCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Product created successfully",
                Data = response
            });
        }

        /// <summary>
        /// Retrieves a product by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The product details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetProductRequest { Id = id };
            var validator = new GetProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetProductCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<GetProductResponse>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = _mapper.Map<GetProductResponse>(response)
            });
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <param name="request">The product update request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated product details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] EditProductRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
                return BadRequest("The ID in the route does not match the ID in the request body.");

            var validator = new EditProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<EditProductCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Product updated successfully"
            });
        }
    }
}
