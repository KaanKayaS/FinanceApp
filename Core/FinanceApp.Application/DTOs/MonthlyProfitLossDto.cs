using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.DTOs
{
    public class MonthlyProfitLossDto
    {
        public decimal Income { get; set; }
        public decimal Expens { get; set; }
        public decimal ProfitLoss { get; set; }
    }
}
