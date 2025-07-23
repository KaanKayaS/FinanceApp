using AutoMapper;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ExpenseRules expenseRules;
        private readonly IMapper mapper;

        public ExpenseService(IUnitOfWork unitOfWork, ExpenseRules expenseRules, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.expenseRules = expenseRules;
            this.mapper = mapper;
        }
        public async Task CreateAsync(CreateExpenseCommand request, int userId)
        {
            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);
            await expenseRules.ExpenseNameNotMustBeSame(expenses, request.Name);

            var expense = new Expens
            {
                Name = request.Name,
                Amount = request.Amount,
                UserId = userId
            };

            await unitOfWork.GetWriteRepository<Expens>().AddAsync(expense);
            await unitOfWork.SaveAsync();
        }

        public async Task<IList<GetAllExpenseAndPaymentByUserQueryResult>> GetAllExpenseAndPaymentByUserAsync(int userId)
        {
            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                predicate: x => x.Memberships.User.Id == userId,
                include: x => x
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.UserId == userId && x.IsPaid == true);

            var mapInstructions = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(expenses);

            var combinedList = mapPayments
                .Concat(mapExpenses)
                .Concat(mapInstructions)
                .ToList();

            return combinedList;
        }

        public async Task<IList<GetAllExpenseByUserQueryResult>> GetAllExpensesByUserAsync(int userId)
        {
            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);
            return mapper.Map<IList<GetAllExpenseByUserQueryResult>>(expenses);
        }

        public async Task<IList<GetLast3ExpenseByUserQueryResult>> GetLast3ExpenseByUserAsync(int userId)
        {
            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                predicate: x => x.Memberships.User.Id == userId,
                include: x => x
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.UserId == userId && x.IsPaid == true);

            var mapInstructions = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(expenses);

            var combinedList = mapPayments
                .Concat(mapExpenses)
                .Concat(mapInstructions)
                .OrderByDescending(x => x.PaidDate)
                .Take(3)
                .ToList();

            return combinedList;
        }

        public async Task<decimal> GetLastMonthExpenseTotalAmountAsync(int userId)
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(
                x => x.UserId == userId && x.CreatedDate >= oneMonthAgo);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                predicate: x => x.Memberships.User.Id == userId && x.CreatedDate >= oneMonthAgo,
                include: x => x
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(
                x => x.UserId == userId && x.IsPaid == true && x.CreatedDate >= oneMonthAgo);

            var mapInstructions = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(expenses);

            var totalAmount = mapPayments
                .Concat(mapExpenses)
                .Concat(mapInstructions)
                .Sum(x => x.Amount);

            return totalAmount;
        }

        public async Task RemoveExpenseAsync(int expenseId, int userId)
        {
            var expense = await unitOfWork.GetReadRepository<Expens>().GetAsync(x => x.Id == expenseId);
            await expenseRules.ExpensNotFound(expense);
            await expenseRules.IsThisYourExpense(expense, userId);

            await unitOfWork.GetWriteRepository<Expens>().HardDeleteAsync(expense);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateExpenseAsync(int expenseId, string name, decimal amount, int userId)
        {
            var expense = await unitOfWork.GetReadRepository<Expens>().GetAsync(x => x.Id == expenseId);
            await expenseRules.ExpensNotFound(expense);
            await expenseRules.IsThisYourExpense(expense, userId);

            expense.Name = name;
            expense.Amount = amount;

            await unitOfWork.GetWriteRepository<Expens>().UpdateAsync(expense);
            await unitOfWork.SaveAsync();
        }
    }
}
