using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class BalanceMemory : EntityBase
    {
        public string Name { get; set; }
        public int CreditCardId { get; set; }
        public decimal Amount { get; set; }
        public AddBalanceCategory AddBalanceCategory { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
