using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Plugins
{
    public class CalculatorPlugin
    {
        [KernelFunction("add")]
        [Description("İki sayısal değer üzerinde toplama işlemi gerçekleştirir.")]
        [return: Description("Toplam değeri döndürür.")]
        public int Add(int number1, int number2)
            => number1 + number2;
    }
}
