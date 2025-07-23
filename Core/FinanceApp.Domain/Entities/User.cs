using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Memberships> Memberships { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
        public ICollection<Expens> Expenses { get; set; }
        public ICollection<Instructions> Instructions { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }


    }
}
