using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Queries.InvestmentPlanQueries;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class InvestmentPlanController : ControllerBase
    {

        private readonly IMediator mediator;

        public InvestmentPlanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvestmentPlan(CreateInvestmentPlanCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Yatırım Planınız başarıyla oluşturuldu.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvestmentPlanByUser()
        {
            var values = await mediator.Send(new GetAllInvestmenPlanByUserQuery());
            return Ok(values);
        }

        [HttpPut]
        public async Task<IActionResult> AddBalancePlan([FromBody] AddBalanceInvestmentPlanCommand command)
        {
            await mediator.Send((command));
            return Ok("Yatırım planınıza para başarıyla eklendi.");
        }


        [HttpDelete]
        public async Task<IActionResult> RemovePlan(int id)
        {
            await mediator.Send(new RemoveInvestmentPlanCommand(id));
            return Ok("Yatırım Planınız başarıyla iptal edildi.");
        }
    }
}
