using FinanceApp.Application.Features.Results.PaymentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<IList<GetPaymentsByCardIdQueryResult>> GetPaymentsByCardIdAsync(int cardId, int userId);
    }
}
