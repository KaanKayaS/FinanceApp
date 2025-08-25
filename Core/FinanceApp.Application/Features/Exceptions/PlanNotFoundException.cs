using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class PlanNotFoundException : BaseException
    {
        public PlanNotFoundException() : base("Böyle bir yatırım planı bulunamadı.") { }
    }
}
