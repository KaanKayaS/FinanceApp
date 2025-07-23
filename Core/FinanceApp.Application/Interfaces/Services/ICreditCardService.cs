using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Results.CreditCardResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface ICreditCardService
    {
        Task AddBalanceAsync(AddBalanceCreditCardCommand request, int userId);
        Task CreateAsync(CreateCreditCardCommand request, int userId);
        Task<IList<GetAllCreditCardsByUserQueryResult>> GetAllByUserAsync(int userId);
        Task RemoveAsync(int creditCardId, int userId);

    }
}
