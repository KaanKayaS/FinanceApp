using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class RemoveInstructionCommandHandler : BaseHandler, IRequestHandler<RemoveInstructionCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly InstructionRules instructionRules;

        public RemoveInstructionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, InstructionRules instructionRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.instructionRules = instructionRules;
        }

        public async Task<Unit> Handle(RemoveInstructionCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Instructions instructions = await unitOfWork.GetReadRepository<Instructions>().GetAsync(x => x.Id == request.Id);
            await instructionRules.InstructionsNotFound(instructions);
            await instructionRules.IsThisYourInstruction(instructions,userId);

            await unitOfWork.GetWriteRepository<Instructions>().HardDeleteAsync(instructions);

            await unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
