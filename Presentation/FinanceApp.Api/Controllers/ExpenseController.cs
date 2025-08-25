using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Queries.MembershipsQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator mediator;

        public ExpenseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(CreateExpenseCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Harcamanız ajandaya başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpens([FromQuery] int id, [FromQuery] decimal amount, [FromQuery] string name)
        {
            var command = new UpdateExpenseCommand
            {
                Id = id,
                Amount = amount,
                Name = name       
            };

            await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, "Harcamanız başarıyla güncellendi.");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveExpens(int id)
        {
            await mediator.Send(new RemoveExpenseCommand(id));
            return Ok("Harcamanız başarıyla silindi");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenseWithPayment()
        {
            var values = await mediator.Send(new GetAllExpenseAndPaymentByUserQuery());
            return Ok(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpense()
        {
            var values = await mediator.Send(new GetAllExpenseByUserQuery());
            return Ok(values);
        }


        [HttpGet]
        public async Task<IActionResult> GetLast3Expense()
        {
            var values = await mediator.Send(new GetLast3ExpenseByUserQuery());
            return Ok(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetLastMonthExpenseTotalAmount()
        {
            var values = await mediator.Send(new GetLastMonthExpenseTotalAmountQuery());
            return Ok(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetDailyNetProfit()
        {
            var result = await mediator.Send(new GetDailyProfitQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TheMostExpensiveExpenseByUser()
        {
            var result = await mediator.Send(new TheMostExpensiveExpenseByUserQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDailyExpenseList()
        {
            var result = await mediator.Send(new GetDailyExpenseListByUserQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyProfitLossByUser()
        {
            var result = await mediator.Send(new MonthlyProfitLossByUserQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyExpenseReport()
        {
            await mediator.Send(new MonthlyExpenseReportQuery());
            return StatusCode(StatusCodes.Status200OK, "Aylık Harcama Raporunuz mailinize başarıyla gönderildi.");
        }
    }
}

