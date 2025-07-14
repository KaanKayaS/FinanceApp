using FinanceApp.Application.Features.Results.MenuResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.MenuQueries
{
    public class GetAllMenuBarQuery : IRequest<IList<GetAllMenuBarQueryResult>>
    {
    }
}
