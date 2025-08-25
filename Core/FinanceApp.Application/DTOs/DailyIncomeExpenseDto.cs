using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.DTOs
{
    public class DailyIncomeExpenseDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public int Count { get; set; }
    }
}
