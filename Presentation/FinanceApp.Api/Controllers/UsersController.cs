using FinanceApp.Application.Features.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfoById() 
        {
            var values = await mediator.Send(new GetUserInfoByIdQuery());
            return Ok(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCount()
        {
            var values = await mediator.Send(new GetUserCountAsyncQuery());
            return Ok(values);
        }


    }
}
