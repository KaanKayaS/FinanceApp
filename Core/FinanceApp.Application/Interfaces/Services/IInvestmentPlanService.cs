using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.InvestmentPlanResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IInvestmentPlanService
    {
        Task CreateAsync(CreateInvestmentPlanCommand command, int userId);
        Task<IList<GetAllInvestmenPlanByUserQueryResult>> GetAllPlanByUserAsync(int userId);
        Task AddBalancePlan(AddBalanceInvestmentPlanCommand command, int userId);
        Task RemoveInvestmentAsync(int investmentId, int userId);
    }
}
