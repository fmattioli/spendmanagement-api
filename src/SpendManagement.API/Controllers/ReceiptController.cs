using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;

namespace SpendManagement.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Add a new receipt on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("addReceipt", Name = nameof(ReceiptController.AddReceipt))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        public async Task<IActionResult> UpdateReceipt(UpdateReceiptInputModel updateReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateReceiptCommand(updateReceiptInputModel), cancellationToken);
            return NoContent();
        }
    }
}
