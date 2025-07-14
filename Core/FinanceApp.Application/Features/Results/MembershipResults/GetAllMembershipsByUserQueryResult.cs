using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.MembershipResult
{
    public class GetAllMembershipsByUserQueryResult
    {
        public string DigitalPlatformName { get; set; }
        public string SubscriptionPlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
