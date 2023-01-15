using MediatR;
using Microsoft.AspNetCore.Mvc;

using Spents.Application.Commands.AddSpent;
using Spents.Application.InputModels;

namespace Spents.API.Controllers
{
    [ApiController]
    [Route("api/spents")]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/addReceipt", Name = nameof(ReceiptController.AddReceipt))]
        public async Task<IActionResult> AddReceipt([FromBody] AddReceiptInputModel addSpentInputModel)
        {
            var receiptId = await _mediator.Send(new AddSpentCommand(addSpentInputModel));
            return Created("/addReceipt", receiptId);
        } 
    }
}
