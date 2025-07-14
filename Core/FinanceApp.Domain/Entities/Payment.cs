using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class Payment : EntityBase
    {
        public int MembershipsId { get; set; }
        public int CreditCardId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow.AddHours(3);
        public Memberships Memberships { get; set; }
        public CreditCard CreditCard { get; set; }

    }
}
