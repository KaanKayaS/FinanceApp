using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.ExpenseResults
{
    public class GetDailyProfitQueryResult
    {
        public DateTime Date { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Net { get; set; } // pozitifse kar, negatifse zarar
    }
}
