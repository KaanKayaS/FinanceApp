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
    public class CreateExpenseCommandHandler : BaseHandler, IRequestHandler<CreateExpenseCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly ExpenseRules expenseRules;

        public CreateExpenseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, ExpenseRules expenseRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.expenseRules = expenseRules;
        }

        public async Task<Unit> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            IList<Expens> expens = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x=> x.UserId == userId);
            await expenseRules.ExpenseNameNotMustBeSame(expens, request.Name);

            await unitOfWork.GetWriteRepository<Expens>().AddAsync(new Expens
            {
               Name = request.Name,
               Amount = request.Amount,
               UserId = userId,
            });

            await unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
