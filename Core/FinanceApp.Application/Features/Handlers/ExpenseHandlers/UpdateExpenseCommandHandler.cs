using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
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

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class UpdateExpenseCommandHandler : BaseHandler, IRequestHandler<UpdateExpenseCommand, Unit>
    {
        private readonly IExpenseService expenseService;
        private readonly AuthRules authRules;

        public UpdateExpenseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IExpenseService expenseService, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.expenseService = expenseService;
            this.authRules = authRules;
        }

        public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await expenseService.UpdateExpenseAsync(request.Id, request.Name, request.Amount, userId);
            return Unit.Value;
        }
    }
}
