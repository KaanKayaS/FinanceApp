using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class ExpenseNameNotMustBeSameException: BaseException
    {
        public ExpenseNameNotMustBeSameException(): base("Bu isimde bir haarcamanız zaten mevcut.") { }
    }
}
