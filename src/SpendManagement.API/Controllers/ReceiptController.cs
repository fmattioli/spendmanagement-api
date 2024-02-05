using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;
using SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt;
using Microsoft.AspNetCore.Authorization;
using SpendManagement.Infra.CrossCutting.Extensions.Filters;
using SpendManagement.Application.Claims;
using SpendManagement.Application.Commands.Receipt.UseCases.AddRecurringReceipt;

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
        /// Edit an existing receipt
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPatch]
        [Route("updateReceipt/{Id:guid}", Name = nameof(UpdateReceipt))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Update")]
        public async Task<IActionResult> UpdateReceipt(UpdateReceiptInputModel updateReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateReceiptCommand(updateReceiptInputModel), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete an existing receipt
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
    }
}
