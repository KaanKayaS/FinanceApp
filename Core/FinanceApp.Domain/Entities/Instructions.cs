using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class Instructions : EntityBase
    {
        public int UserId { get; set; }
        public string Title { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime ScheduledDate { get; set; } 
        public bool IsPaid { get; set; } 
        public string? Description { get; set; }
        public Guid? GroupId { get; set; } 
        public User User { get; set; }
    }
}
