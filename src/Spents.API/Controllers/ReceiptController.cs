using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spents.Application.Commands.AddReceipt;
using Spents.Application.InputModels;

namespace Spents.API.Controllers
{
    [ApiController]
    [Route("api/spents")]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Add a new receipt
        /// Required recept body
        /// </summary>
        /// <returns>Add a new receipt to the platform.</returns>
        [HttpPost]
        [Route("/addReceipt", Name = nameof(ReceiptController.AddReceipt))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddReceipt([FromBody] ReceiptInputModel addSpentInputModel)
        {
            var receiptId = await _mediator.Send(new AddReceiptCommand(addSpentInputModel));
            return Created("/addReceipt", receiptId);
        } 
    }
}
