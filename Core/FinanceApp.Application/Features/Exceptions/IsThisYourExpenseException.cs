using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class IsThisYourExpenseException : BaseException
    {
        public IsThisYourExpenseException(): base("Bu Harcama size ait değil işlem yapamazsınız") { }
    }
}
