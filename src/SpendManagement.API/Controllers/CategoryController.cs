using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Commands.Category.UseCases.AddCategory;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Commands.Category.UseCases.UpdateCategory;
using SpendManagement.Application.Commands.Category.UseCases.DeleteCategory;
using Microsoft.AspNetCore.Authorization;

namespace SpendManagement.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Add a new Category on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("addCategory", Name = nameof(AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryInputModel categoryInputModel, CancellationToken cancellationToken)
        {
            var categoryId = await _mediator.Send(new AddCategoryCommand(categoryInputModel), cancellationToken);
            return Created("/addCategory", categoryId);
        }

        /// <summary>
        /// Update an existing category on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPatch]
        [Route("updateCategory/{Id:guid}", Name = nameof(UpdateCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryInputModel categoryInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateCategoryCommand(categoryInputModel), cancellationToken);
            return Accepted();
        }

        /// <summary>
        /// Delete an existing category on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpDelete]
        [Route("deleteCategory/{Id:guid}", Name = nameof(DeleteCategory))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCategoryCommand(Id), cancellationToken);
            return Accepted();
        }
    }
}
