using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class InstructionController : ControllerBase
    {
        private readonly IMediator mediator;

        public InstructionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstruction(CreateInstructionCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Talimatınız başarıyla eklendi.");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateInstruction(UpdateInstructionCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, "Talimatınız başarıyla güncellendi.");
        }

        [HttpPut]
        public async Task<IActionResult> SetPaidTrueInstruction(SetPaidTrueInstructionCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, "Talimatınız ödendi olarak işaretlendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveInstruction(int id)
        {
            await mediator.Send(new RemoveInstructionCommand(id));
            return StatusCode(StatusCodes.Status200OK, "Talimatınız başarıyla silindi.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInstruction()
        {
            var values = await mediator.Send(new GetAllInstructionsByUserQuery());
            return Ok(values);
        }
    }
}
