using FinanceApp.Application.Features.Results.CreditCardResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.CreditCardQueries
{
    public class GetAllCreditCardsByUserQuery : IRequest<IList<GetAllCreditCardsByUserQueryResult>>
    {
    }
}
