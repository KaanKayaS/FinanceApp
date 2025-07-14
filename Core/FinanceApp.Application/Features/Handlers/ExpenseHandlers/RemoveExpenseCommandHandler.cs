using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
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

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class RemoveExpenseCommandHandler : BaseHandler, IRequestHandler<RemoveExpenseCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly ExpenseRules expenseRules;

        public RemoveExpenseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, ExpenseRules expenseRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.expenseRules = expenseRules;
        }

        public async Task<Unit> Handle(RemoveExpenseCommand request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            Expens expens = await unitOfWork.GetReadRepository<Expens>().GetAsync(x => x.Id == request.Id);
            await expenseRules.ExpensNotFound(expens);
            await expenseRules.IsThisYourExpense(expens, userId);

            await unitOfWork.GetWriteRepository<Expens>().HardDeleteAsync(expens);

            await unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
