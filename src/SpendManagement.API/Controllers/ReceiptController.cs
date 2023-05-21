using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Commands.AddReceipt;
using SpendManagement.Application.Commands.UpdateReceipt;
using SpendManagement.Application.Commands.UpdateReceiptItem;
using SpendManagement.Application.InputModels.Common;

namespace SpendManagement.API.Controllers
{
    [Route("api/")]
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
        [Route("/addReceipt", Name = nameof(ReceiptController.AddReceipt))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddReceipt([FromBody] ReceiptInputModel addSpentInputModel, CancellationToken cancellationToken)
        {
            var receiptId = await _mediator.Send(new AddReceiptCommand(addSpentInputModel), cancellationToken);
            return Created("/addReceipt", receiptId);
        }

        /// <summary>
        /// Add a new Category on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("/addCategory", Name = nameof(ReceiptController.AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryInputModel categoryInputModel, CancellationToken cancellationToken)
        {
            var receiptId = await _mediator.Send(null, cancellationToken);
            return Created("/addCategory", receiptId);
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

        /// <summary>
        /// Edit an existing Item on a receipt
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPatch]
        [Route("updateReceiptItem/{Id:guid}", Name = nameof(UpdateReceiptItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateReceiptItem(UpdateReceiptItemInputModel updateReceiptInputModel, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateReceiptItemCommand(updateReceiptInputModel), cancellationToken);
            return NoContent();
        }
    }
}
