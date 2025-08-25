using FinanceApp.Application.Features.Results.InvestmentPlanResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.InvestmentPlanQueries
{
    public class GetAllInvestmenPlanByUserQuery : IRequest<IList<GetAllInvestmenPlanByUserQueryResult>>
    {
    }
}
