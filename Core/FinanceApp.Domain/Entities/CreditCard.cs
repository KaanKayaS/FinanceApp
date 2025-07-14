using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class CreditCard : EntityBase
    {
        public int UserId { get; set; }
        public string CardNo { get; set; }
        public string ValidDate { get; set; }
        public string Cvv { get; set; }
        public string NameOnCard { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<BalanceMemory> BalanceMemories { get; set; }
    }
}
