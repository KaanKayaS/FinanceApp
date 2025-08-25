using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.DTOs
{
    public class EmailConfirmationMessage
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
