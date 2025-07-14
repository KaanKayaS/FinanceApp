using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.DigitalPlatformQueries
{
    public class GetAllDigitalPlatfomQuery : IRequest<IList<GetAllDigitalPlatfomQueryResult>>
    {
    }
}
