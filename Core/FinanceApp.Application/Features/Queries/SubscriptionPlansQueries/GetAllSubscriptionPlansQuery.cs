using FinanceApp.Application.Features.Results.SubscriptionPlansResults;
using FinanceApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.SubscriptionPlansQueries
{
    public class GetAllSubscriptionPlansQuery : IRequest<GetAllSubscriptionPlansQueryResult>
    {
        public int DigitalPlatformId { get; set; }
        public SubscriptionType PlanType { get; set; } // Aylık, Yıllık vb.

    }
}
