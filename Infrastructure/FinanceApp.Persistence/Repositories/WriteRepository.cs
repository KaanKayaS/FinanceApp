using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, new()
    {
        private readonly AppDbContext appDbContext;

        public WriteRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(T entity)
        {
            await appDbContext.AddAsync(entity);
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            await appDbContext.AddRangeAsync(entities);
        }

        public Task HardDeleteAsync(T entity)
        {
             appDbContext.Remove(entity);
             return Task.CompletedTask;
        }

        public Task HardDeleteRangeAsync(IList<T> entity)
        {
            appDbContext.RemoveRange(entity);
            return Task.CompletedTask;
        }

        public Task<T> UpdateAsync(T entity)
        {
            appDbContext.Update(entity);
            return Task.FromResult(entity);
        }
    }
}
