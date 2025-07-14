using FinanceApp.Application.Features.Commands.MembershipsCommands;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Queries;
using FinanceApp.Application.Features.Queries.MembershipsQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMediator mediator;

        public MembershipController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMembership(CreateMembershipCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Üyeliğiniz başarıyla oluşturuldu");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> RemoveMembership(int id)
        {
            await mediator.Send(new RemoveMembershipCommand(id));
            return Ok("Üyeliğiniz başarıyla iptal edildi");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllMembershipsByUser()
        {
            var values = await mediator.Send(new GetAllMembershipsByUserQuery());
            return Ok(values);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMembershipCount()
        {
            var values = await mediator.Send(new GetMembershipCountByUserQuery());
            return Ok(values);
        }
    }
}
