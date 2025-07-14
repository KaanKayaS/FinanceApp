using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class ExpenseRules : BaseRules
    {
        public Task ExpenseNameNotMustBeSame(IList<Expens> expens, string expenseName)
        {
            foreach( var expense in expens ) 
            {
                if (expense.Name.Equals(expenseName, StringComparison.OrdinalIgnoreCase))
                    throw new ExpenseNameNotMustBeSameException();

            }
            return Task.CompletedTask;
        }

        public Task IsThisYourExpense(Expens expens, int UserId)
        {
           if (expens.UserId != UserId)
                    throw new IsThisYourExpenseException();
       
            return Task.CompletedTask;
        }

        public Task ExpensNotFound(Expens expens)
        {
            if (expens == null)
                throw new ExpensNotFoundException();

            return Task.CompletedTask;
        }

    }
}
