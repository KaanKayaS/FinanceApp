using FinanceApp.Application.Features.Results.MembershipResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.MembershipsQueries
{
    public class GetAllMembershipsByUserQuery : IRequest<IList<GetAllMembershipsByUserQueryResult>>
    {
    }
}
