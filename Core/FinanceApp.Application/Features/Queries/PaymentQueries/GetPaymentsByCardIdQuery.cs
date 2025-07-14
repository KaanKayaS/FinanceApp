using FinanceApp.Application.Features.Results.PaymentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Queries.PaymentQueries
{
    public class GetPaymentsByCardIdQuery : IRequest<IList<GetPaymentsByCardIdQueryResult>>
    {
        public int Id { get; set; }
        public GetPaymentsByCardIdQuery(int id)
        {
            Id = id;
        }
    }
}
