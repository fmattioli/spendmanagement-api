using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Claims;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;
using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.DeleteRecurringReceipt;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.UpdateRecurringReceipt;
using SpendManagement.Infra.CrossCutting.Extensions.Filters;

namespace SpendManagement.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class ReceiptController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Add a new receipt on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("addReceipt", Name = nameof(ReceiptController.AddReceipt))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Insert")]
        public async Task<IActionResult> AddReceipt([FromBody] ReceiptInputModel addSpentInputModel, CancellationToken cancellationToken)
        {
            var receiptId = await _mediator.Send(new AddReceiptCommand(addSpentInputModel), cancellationToken);
            return Created("/addReceipt", receiptId);
        }

        /// <summary>
        /// Edit an existing receipt on the platform
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPatch]
        [Route("updateReceipt/{Id}", Name = nameof(UpdateReceipt))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Update")]
        public async Task<IActionResult> UpdateReceipt(Guid Id, UpdateReceiptInputModel updateReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateReceiptCommand(Id, updateReceiptInputModel), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete an existing receipt on the platform
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpDelete]
        [Route("deleteReceipt/{Id:guid}", Name = nameof(DeleteReceipt))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorizeAttribute(ClaimTypes.Receipt, "Delete")]
        public async Task<IActionResult> DeleteReceipt(Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteReceiptCommand(Id), cancellationToken);
            return Accepted();
        }

        /// <summary>
        /// Add a new recurring receipt on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("addRecurringReceipt", Name = nameof(AddRecurringReceipt))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Insert")]
        public async Task<IActionResult> AddRecurringReceipt([FromBody] RecurringReceiptInputModel recurringReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddRecurringReceiptCommand(recurringReceiptInputModel), cancellationToken);
            return Accepted();
        }

        /// <summary>
        /// Edit an existing recurring  receipt on the platform
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPatch]
        [Route("updateRecurringReceipt/{Id}", Name = nameof(UpdateRecurringReceipt))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Update")]
        public async Task<IActionResult> UpdateRecurringReceipt(Guid Id, UpdateRecurringReceiptInputModel updateReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateRecurringReceiptCommand(Id, updateReceiptInputModel), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete an existing recurring  receipt on the platform
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpDelete]
        [Route("deleteRecurringReceipt/{Id:guid}", Name = nameof(DeleteRecurringReceipt))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Delete")]
        public async Task<IActionResult> DeleteRecurringReceipt(Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteRecurringReceiptCommand(Id), cancellationToken);
            return Accepted();
        }
    }
}
