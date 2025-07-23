using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class AuditLog : EntityBase
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ActionName { get; set; }
        public string RequestData { get; set; }
        public string? ResponseData { get; set; }
        public int DurationMs { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow.AddHours(3);    
        public User? User { get; set; }
    }
} 