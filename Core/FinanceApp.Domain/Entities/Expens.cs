using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class Expens : EntityBase
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
