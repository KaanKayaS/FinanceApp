using FinanceApp.Application.Features.Queries.DigitalPlatformQueries;
using FinanceApp.Application.Features.Queries.SubscriptionPlansQueries;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubscriptionPlanController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetSubPlanPrice([FromQuery] int digitalPlatformId, [FromQuery] SubscriptionType planType)
        {
            var query = new GetAllSubscriptionPlansQuery
            {
                DigitalPlatformId = digitalPlatformId,
                PlanType = planType
            };

            var value = await mediator.Send(query);
            return Ok(value);
        }
    }
}
