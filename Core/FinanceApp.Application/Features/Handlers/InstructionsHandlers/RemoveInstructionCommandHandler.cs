using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly IInstructionService instructionService;

        public RemoveInstructionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules,IInstructionService instructionService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.instructionService = instructionService;
        }

        public async Task<Unit> Handle(RemoveInstructionCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await instructionService.RemoveInstructionAsync(request.Id, userId);

            return Unit.Value;
        }
    }
}
