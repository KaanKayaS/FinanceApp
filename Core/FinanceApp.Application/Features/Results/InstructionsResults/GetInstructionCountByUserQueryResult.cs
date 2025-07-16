using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.InstructionsResults
{
    public class GetInstructionCountByUserQueryResult
    {
        public int TotalInstruction { get; set; }
        public int WaitingInstruction { get; set; }
        public int PaidInstruction { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
