using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Results.ExpenseResults;
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
    }
}
