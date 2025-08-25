using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Hangfire
{
    public interface IMembershipRenewalService
    {
        Task RenewMembershipsAsync();
    }
}
