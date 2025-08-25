using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class SystemSettings : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
