using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.ExpenseCommands
{
    public class RemoveExpenseCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public RemoveExpenseCommand(int id)
        {
            Id = id;
        }
    }
}
