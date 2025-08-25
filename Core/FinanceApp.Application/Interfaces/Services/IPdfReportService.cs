using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Results.ExpenseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IPdfReportService
    {
        byte[] GenerateExpensePdf(IList<GetAllExpenseAndPaymentByUserQueryResult> expenses);
        byte[] GenerateInstructionPdf(IList<InstructionDto> instructions);
    }
}
