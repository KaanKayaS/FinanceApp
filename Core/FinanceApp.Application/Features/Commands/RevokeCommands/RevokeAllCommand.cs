using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.RevokeCommands
{
    public class RevokeAllCommand : IRequest<Unit>
    {
    }
}
