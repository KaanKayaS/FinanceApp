using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Results.ExpenseResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.ExpenseQueries
{
    public class GetDailyProfitQuery : IRequest<IList<GetDailyProfitQueryResult>>
    {
    }
}
