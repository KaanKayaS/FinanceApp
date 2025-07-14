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
    public class UpdateInstructionCommandHandler : BaseHandler, IRequestHandler<UpdateInstructionCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly InstructionRules instructionRules;

        public UpdateInstructionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, InstructionRules instructionRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.instructionRules = instructionRules;
        }

        public async Task<Unit> Handle(UpdateInstructionCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Instructions instructions = await unitOfWork.GetReadRepository<Instructions>().GetAsync(x => x.Id == request.Id);
            await instructionRules.InstructionsNotFound(instructions);
            await instructionRules.IsThisYourInstruction(instructions, userId);

            instructions.Title = request.Title;
            instructions.Description = request.Description;
            instructions.ScheduledDate = request.ScheduledDate;
            instructions.Amount = request.Amount;
            

            await unitOfWork.GetWriteRepository<Instructions>().UpdateAsync(instructions);

            await unitOfWork.SaveAsync();
            return Unit.Value; 
        }
    }
}
