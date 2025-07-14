using FinanceApp.Application.Features.Results.ExpenseResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.ExpenseQueries
{
    public class GetLast3ExpenseByUserQuery : IRequest<IList<GetLast3ExpenseByUserQueryResult>>
    {
    }
}
