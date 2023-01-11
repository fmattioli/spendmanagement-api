using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spents.Application.Commands.AddSpent;
using System.Linq;
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
        public async Task<IActionResult> AddSpent(AddSpentInputModel addSpentInputModel)
        {
            var addSpentCommand = addSpentInputModel.ToCommand();
            await _mediator.Send(addSpentCommand);
            return Ok();
        } 
    }
}
