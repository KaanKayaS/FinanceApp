using FinanceApp.Application.Features.Results.MenuResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IMenuService
    {
        Task<IList<GetAllMenuBarQueryResult>> GetAllMenusAsync();
    }
}
