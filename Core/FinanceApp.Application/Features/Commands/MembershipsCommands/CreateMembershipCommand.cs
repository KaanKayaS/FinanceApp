using FinanceApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.MembershipsCommands
{
    public class CreateMembershipCommand : IRequest<Unit>
    {
        public int DigitalPlatformId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int CreditCardId { get; set; }
        public bool IsAutoRenewal { get; set; }
    }

}
