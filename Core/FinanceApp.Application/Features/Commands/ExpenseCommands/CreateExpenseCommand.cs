using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.ExpenseCommands
{
    public class CreateExpenseCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
