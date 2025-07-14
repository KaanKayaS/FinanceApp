using FinanceApp.Application.Features.Queries.DigitalPlatformQueries;
using FinanceApp.Application.Features.Queries.MembershipsQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DigitalPlatformController : ControllerBase
    {
        private readonly IMediator mediator;

        public DigitalPlatformController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDigitalPlatform()
        {
            var values = await mediator.Send(new GetAllDigitalPlatfomQuery());
            return Ok(values);
        }
    }
}
