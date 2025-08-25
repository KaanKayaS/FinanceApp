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
using FinanceApp.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace FinanceApp.Persistence.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ExpenseRules expenseRules;
        private readonly IMapper mapper;
        private readonly ICreditCardService creditCardService;
        private readonly IPdfReportService expenseReportService;
        private readonly IMailService mailService;
        private readonly UserManager<User> userManager;

        public ExpenseService(IUnitOfWork unitOfWork, ExpenseRules expenseRules, IMapper mapper, ICreditCardService creditCardService, IPdfReportService expenseReportService, IMailService mailService, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.expenseRules = expenseRules;
            this.mapper = mapper;
            this.creditCardService = creditCardService;
            this.expenseReportService = expenseReportService;
            this.mailService = mailService;
            this.userManager = userManager;
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
            var cards = await creditCardService.GetAllByUserAsync(userId);
            var cardIds = cards.Select(card => card.CardId).ToList();
            var piggyBank = await unitOfWork.GetReadRepository<BalanceMemory>().GetAllAsync(x => cardIds.Contains(x.CreditCardId)
                                                                                         && x.AddBalanceCategory == AddBalanceCategory.PiggyBank);

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
            var mappiggyBank = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(piggyBank);

            var combinedList = mapPayments
                .Concat(mapExpenses)
                .Concat(mapInstructions)
                .Concat(mappiggyBank)
                .OrderByDescending(x => x.PaidDate)
                .ToList();

            return combinedList;
        }

        public async Task<IList<DailyIncomeExpenseDto>> GetAllIncomeForDateTime(int userId)             // Gelirleri dateine göre gruplayarak getiren method
        {
            var cards = await creditCardService.GetAllByUserAsync(userId);
            var cardIds = cards.Select(card => card.CardId).ToList();

            var IncomeList = await unitOfWork.GetReadRepository<BalanceMemory>().GetAllAsync(x => cardIds.Contains(x.CreditCardId) 
                                                                                          && x.AddBalanceCategory != AddBalanceCategory.PiggyBank);

            var grouped = IncomeList
            .GroupBy(x => x.CreatedDate.Date)
            .Select(g => new DailyIncomeExpenseDto
            {
                Date = g.Key,
                TotalAmount = g.Sum(x => x.Amount), 
                Count = g.Count()
            })
            .OrderByDescending(x => x.Date)
            .ToList();

            return grouped;
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

        public async Task<IList<DailyIncomeExpenseDto>> GetAllExpenseForDateTime(int userId)    // Giderleri dateine göre gruplayarak getiren method
        {
            var expenses = await GetAllExpenseAndPaymentByUserAsync(userId);

            var grouped = expenses
               .GroupBy(x => x.PaidDate.Date)
               .Select(g => new DailyIncomeExpenseDto
               {
                   Date = g.Key,
                   TotalAmount = g.Sum(x => x.Amount),
                   Count = g.Count()
               })
               .OrderByDescending(x => x.Date)
               .ToList();

            return grouped;
        }

        public async Task<IList<GetDailyProfitQueryResult>> GetAllDailyProfitForDateTime(int userId)  // profit kar zarar getirir.
        {
            var incomeList = await GetAllIncomeForDateTime(userId);
            var expenseList = await GetAllExpenseForDateTime(userId);

            var allDates = incomeList.Select(x => x.Date)
                            .Union(expenseList.Select(x => x.Date))
                            .Distinct()
                            .OrderByDescending(d => d)
                            .ToList();

            var result = allDates.Select(date =>
            {
                var income = incomeList.FirstOrDefault(x => x.Date == date)?.TotalAmount ?? 0;
                var expense = expenseList.FirstOrDefault(x => x.Date == date)?.TotalAmount ?? 0;

                return new GetDailyProfitQueryResult
                {
                    Date = date,
                    Income = income,
                    Expense = expense,
                    Net = income - expense
                };
            }).ToList();

            return result;
        }

        public async Task<TheMostExpensiveExpenseDto> TheMostExpensiveExpenseByUser(int userId)
        {
            var list = await GetAllExpenseAndPaymentByUserAsync(userId);

            var TheMostExpensiveExpense = list
               .OrderByDescending(x => x.Amount)
               .FirstOrDefault();

            return mapper.Map<TheMostExpensiveExpenseDto>(TheMostExpensiveExpense);
        }

        public async Task<MonthlyProfitLossDto> MonthlyProfitLossByUser(int userId)
        {
            var list = await GetAllExpenseAndPaymentByUserAsync(userId);

            var monthAgo = DateTime.UtcNow.AddMonths(-1);
            var now = DateTime.UtcNow.AddHours(3);

            var totalExpenseLastMonth = list
                .Where(x => x.PaidDate > monthAgo && x.PaidDate < now)
                .Sum(x => x.Amount);

            var cards = await creditCardService.GetAllByUserAsync(userId);
            var cardIds = cards.Select(card => card.CardId).ToList();

            var IncomeList = await unitOfWork.GetReadRepository<BalanceMemory>().GetAllAsync(x => cardIds.Contains(x.CreditCardId)
                                                                                          && x.AddBalanceCategory != AddBalanceCategory.PiggyBank
                                                                                          && x.CreatedDate > monthAgo
                                                                                          && x.CreatedDate < now);
            var totalIncomeLastMonth = IncomeList.Sum(x => x.Amount);

            return new MonthlyProfitLossDto
            {
                Expens = totalExpenseLastMonth,
                Income = totalIncomeLastMonth,
                ProfitLoss = totalIncomeLastMonth - totalExpenseLastMonth
            };
        }

        public async Task<IList<GetAllExpenseAndPaymentByUserQueryResult>> MonthlyExpenseReport(int userId)
        {
            var combinedList = await GetAllExpenseAndPaymentByUserAsync(userId);

            var now = DateTime.UtcNow.AddHours(3);
            var monthAgo = now.AddMonths(-1);

            var filteredList = combinedList
                   .Where(x => x.PaidDate >= monthAgo && x.PaidDate <= now)
                   .ToList();

            return filteredList;
        }

        public async Task<Unit> GeneratePdfMonthlyExpenseReport(int userId)
        {
           var list = await MonthlyExpenseReport(userId);

           User? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

           var pdfBytes = expenseReportService.GenerateExpensePdf(list);

           using var pdfStream = new MemoryStream(pdfBytes);

           string subject = "Harcama Raporu";
           string bodyContent = "Son bir ayda yaptığınız harcama raporu ektedir. İndirebilirsiniz";

            await mailService.SendMailAsync(
                 user.Email,
                 subject,
                 bodyContent,
                 true,
                 attachments: new List<(Stream, string)>
                 {
                    (pdfStream, "HarcamaRaporu.pdf")
                 }
             );
            return Unit.Value;
        }
    }
}
