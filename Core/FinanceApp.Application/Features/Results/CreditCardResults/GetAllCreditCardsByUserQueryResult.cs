using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.CreditCardResults
{
    public class GetAllCreditCardsByUserQueryResult
    {
        public int CardId { get; set; }
        public string CardNo { get; set; }
        public string ValidDate { get; set; }
        public string Cvv { get; set; }
        public string NameOnCard { get; set; }
        public decimal Balance { get; set; }
    }
}
