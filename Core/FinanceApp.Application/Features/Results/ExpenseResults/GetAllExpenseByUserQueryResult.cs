using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.ExpenseResults
{
    public class GetAllExpenseByUserQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
