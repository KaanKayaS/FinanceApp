using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class CreditCardNoNotBeSameException : BaseException
    {
        public CreditCardNoNotBeSameException() : base("Bu Kart numarasına ait kart zaten sistemde kayıtlı.") { }

    }
}
