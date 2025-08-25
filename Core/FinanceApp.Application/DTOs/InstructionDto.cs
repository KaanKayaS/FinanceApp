using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.DTOs
{
    public class InstructionDto
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
