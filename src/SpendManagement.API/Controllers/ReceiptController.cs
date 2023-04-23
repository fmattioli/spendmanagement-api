using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.Commands.AddReceipt;
using SpendManagement.Application.InputModels;

namespace SpendManagement.API.Controllers
{
    [ApiController]
    [Route("api/SpendManagement")]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Add a new receipt
        /// Required receipt body
        /// </summary>
        /// <returns>Add a new receipt to the platform.</returns>
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
    }
}
