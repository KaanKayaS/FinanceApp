using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Queries.MenuQueries;
using FinanceApp.Application.Features.Queries.PaymentQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMediator mediator;
        public MenuController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccountMenuBar()
        {
            var values = await mediator.Send(new GetAllMenuBarQuery());
            return Ok(values);
        }
    }
}
