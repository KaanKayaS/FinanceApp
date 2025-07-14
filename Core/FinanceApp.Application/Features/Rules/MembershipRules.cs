using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class MembershipRules : BaseRules
    {
        public Task AlreadyMembership(Memberships membership)
        {     
            if (membership != null)
                throw new AlreadyMembershipException();
            
            return Task.CompletedTask;
        }

        public Task MembershipNotFound(Memberships membership)
        {
            if (membership == null)
                throw new MembershipNotFoundException();

            return Task.CompletedTask;
        }
    }
}
