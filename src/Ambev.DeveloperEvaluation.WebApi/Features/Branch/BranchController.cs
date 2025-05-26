using Ambev.DeveloperEvaluation.Application.Branch.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branch.EditBranch;
using Ambev.DeveloperEvaluation.Application.Branch.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branch.InactiveBranch;
using Ambev.DeveloperEvaluation.Application.Branch.ListBranch;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branch.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branch.EditBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branch.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branch.InactiveBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branch.ListBranch;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branch
{
    /// <summary>
    /// Controller for managing branch operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BranchController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new branch
        /// </summary>
        /// <param name="request">The branch creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created branch details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateBranchCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Branch created successfully",
                Data = response
            });
        }

        /// <summary>
        /// Retrieves a branch by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the branch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The branch details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetBranchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBranch([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetBranchRequest { Id = id };
            var validator = new GetBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetBranchCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<GetBranchResponse>
            {
                Success = true,
                Message = "Branch retrieved successfully",
                Data = _mapper.Map<GetBranchResponse>(response)
            });
        }

        /// <summary>
        /// Updates an existing branch
        /// </summary>
        /// <param name="id">The unique identifier of the branch</param>
        /// <param name="request">The branch update request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated branch details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBranch([FromRoute] Guid id, [FromBody] EditBranchRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
                return BadRequest("The ID in the route does not match the ID in the request body.");

            var validator = new EditBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<EditBranchCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Branch updated successfully"
            });
        }

        /// <summary>
        /// Retrieves a paginated list of branches
        /// </summary>
        /// <param name="request">Pagination and search parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged list of branches</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<PagedResult<ListBranchResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedBranches([FromQuery] ListBranchRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ListBranchCommand>(request);
            var pagedResult = await _mediator.Send(command, cancellationToken);

            var response = new PagedResult<ListBranchResponse>
            {
                Items = _mapper.Map<IEnumerable<ListBranchResponse>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount
            };

            return Ok(new ApiResponseWithData<PagedResult<ListBranchResponse>>
            {
                Success = true,
                Message = "Branches retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Inactivates a branch by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the branch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The branch details if found</returns>
        [HttpPut("{id}/inactive")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InactiveBranch([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new InactiveBranchRequest { Id = id };
            var validator = new InactiveBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<InactiveBranchCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Branch inactivated successfully"
            });
        }
    }
}
