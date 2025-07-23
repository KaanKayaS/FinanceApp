using AutoMapper;
using FinanceApp.Application.Features.Results.MenuResults;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public MenuService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IList<GetAllMenuBarQueryResult>> GetAllMenusAsync()
        {
            var menus = await unitOfWork.GetReadRepository<Menu>().GetAllAsync();
            return mapper.Map<IList<GetAllMenuBarQueryResult>>(menus);
        }
    }
}
