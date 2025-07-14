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
    public class CreateInstructionCommandHandler : BaseHandler, IRequestHandler<CreateInstructionCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly InstructionRules instructionRules;

        public CreateInstructionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, InstructionRules instructionRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.instructionRules = instructionRules;
        }

        public async Task<Unit> Handle(CreateInstructionCommand request, CancellationToken cancellationToken)
        {
            
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var instruction = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.UserId == userId);
            await instructionRules.InstructionNameNotMustBeSame(instruction, request.Title);

            await unitOfWork.GetWriteRepository<Instructions>().AddAsync(new Instructions
            {
                Title = request.Title,
                Amount = request.Amount,
                ScheduledDate = request.ScheduledDate,
                IsPaid = false,
                Description = request.Description,
                UserId = userId,
            }); ;

            await unitOfWork.SaveAsync();

            return Unit.Value;                     
        
        }
    }
}
