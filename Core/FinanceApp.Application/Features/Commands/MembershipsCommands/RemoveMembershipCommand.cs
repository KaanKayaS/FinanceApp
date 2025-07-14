using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.MembershipsCommands
{
    public class RemoveMembershipCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public RemoveMembershipCommand(int id)
        {
            Id = id;
        }
    }
}
