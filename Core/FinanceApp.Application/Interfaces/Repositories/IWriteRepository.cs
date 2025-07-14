using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Repositories
{
    public interface IWriteRepository<T> where T : class, new()
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(IList<T> entities);

        Task<T> UpdateAsync(T entity);

        Task HardDeleteAsync(T entity);

        Task HardDeleteRangeAsync(IList<T> entity);
    }
}
