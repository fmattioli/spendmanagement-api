using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spents.Application.Commands.AddReceipt;
using Spents.Application.InputModels;
using Spents.Application.Queries.GetReceipts;

namespace Spents.API.Controllers
{
    [ApiController]
    [Route("api/spents")]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("/getReceipts", Name = nameof(ReceiptController.GetReceipts))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReceipts([FromQuery] GetReceiptsViewModel getReceiptsViewModel)
        {
            var receiptId = await _mediator.Send(new GetReceiptsQuery(getReceiptsViewModel));
            return Created("/addReceipt", receiptId);
        }

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
