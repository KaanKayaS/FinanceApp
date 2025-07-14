using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class IsThisYourInstructionException : BaseException
    {
        public IsThisYourInstructionException(): base("Bu Talimat size ait değil. İşlem yapamazsınız.") { }
    }
}
