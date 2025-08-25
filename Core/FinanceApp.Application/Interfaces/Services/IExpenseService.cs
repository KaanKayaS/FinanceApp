using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IExpenseService
    {
        Task CreateAsync(CreateExpenseCommand request, int userId);
        Task<IList<GetAllExpenseAndPaymentByUserQueryResult>> GetAllExpenseAndPaymentByUserAsync(int userId);
        Task<IList<GetAllExpenseByUserQueryResult>> GetAllExpensesByUserAsync(int userId);
        Task<IList<GetLast3ExpenseByUserQueryResult>> GetLast3ExpenseByUserAsync(int userId);
        Task<decimal> GetLastMonthExpenseTotalAmountAsync(int userId);
        Task RemoveExpenseAsync(int expenseId, int userId);
        Task UpdateExpenseAsync(int expenseId, string name, decimal amount, int userId);
        Task<IList<DailyIncomeExpenseDto>> GetAllIncomeForDateTime(int userId);   // gelirleri dateine göre gruplayarak getiren method
        Task<IList<DailyIncomeExpenseDto>> GetAllExpenseForDateTime(int userId);  // giderleri dateine göre gruplayarak getiren method
        Task<IList<GetDailyProfitQueryResult>> GetAllDailyProfitForDateTime(int userId);  // profit kar zarar 
        Task<TheMostExpensiveExpenseDto> TheMostExpensiveExpenseByUser(int userId);
        Task<MonthlyProfitLossDto> MonthlyProfitLossByUser(int userId);
        Task<IList<GetAllExpenseAndPaymentByUserQueryResult>> MonthlyExpenseReport(int userId);
        Task<Unit> GeneratePdfMonthlyExpenseReport(int userId);
    }
}
