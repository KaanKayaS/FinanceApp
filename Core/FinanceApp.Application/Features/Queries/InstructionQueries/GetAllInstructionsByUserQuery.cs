using FinanceApp.Application.Features.Results.InstructionsResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.InstructionQueries
{
    public class GetAllInstructionsByUserQuery : IRequest<IList<GetAllInstructionsByUserQueryResult>>
    {
    }
}
