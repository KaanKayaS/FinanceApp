using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.CreditCardCommands
{
    public class AddBalanceCreditCardCommand : IRequest<Unit>
    {
        public  int Id { get; set; }
        public decimal Balance { get; set; }

    }
}
