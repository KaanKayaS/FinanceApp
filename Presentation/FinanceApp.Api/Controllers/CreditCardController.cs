using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Queries.CreditCardQueries;
using FinanceApp.Application.Features.Queries.PaymentQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {

        private readonly IMediator mediator;
        public CreditCardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCreditCard(CreateCreditCardCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Kredi kartınız başarıyla eklendi.");
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AddBalance([FromQuery] int id, [FromQuery] decimal balance)
        {
            var command = new AddBalanceCreditCardCommand
            {
                Id = id,
                Balance = balance
            };

            await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, "Kredi kartınıza bakiye başarıyla yüklendi.");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveCreditCard(int id)
        {
            await mediator.Send(new RemoveCreditCardCommand(id));
            return Ok("Kredi kartınız başarıyla silindi");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCreditCardByUser()
        {
            var values = await mediator.Send(new GetAllCreditCardsByUserQuery());
            return Ok(values);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAccountTransactions(int id)
        {
            var values = await mediator.Send(new GetPaymentsByCardIdQuery(id));
            return Ok(values);
        }
    }
}
