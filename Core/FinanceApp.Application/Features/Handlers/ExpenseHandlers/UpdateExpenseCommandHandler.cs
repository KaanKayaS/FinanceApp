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
    public class UpdateExpenseCommandHandler : BaseHandler, IRequestHandler<UpdateExpenseCommand, Unit>
    {
        private readonly ExpenseRules expenseRules;
        private readonly AuthRules authRules;

        public UpdateExpenseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            ExpenseRules expenseRules, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.expenseRules = expenseRules;
            this.authRules = authRules;
        }

        public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {      
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            Expens expens = await unitOfWork.GetReadRepository<Expens>().GetAsync(x => x.Id == request.Id);
            await expenseRules.ExpensNotFound(expens);
            await expenseRules.IsThisYourExpense(expens, userId);

            expens.Name = request.Name;
            expens.Amount = request.Amount;

            await unitOfWork.GetWriteRepository<Expens>().UpdateAsync(expens);

            await unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
