using MediatR;
using Microsoft.AspNetCore.Mvc;

using Spents.Application.Commands.AddSpent;
using Spents.Application.InputModels;

namespace Spents.API.Controllers
{
    [Route("api/[controller]")]
    public class SpentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddSpent([FromBody] AddSpentInputModel addSpentInputModel)
        {
            await _mediator.Send(new AddSpentCommand(addSpentInputModel));
            return Ok();
        } 
    }
}
