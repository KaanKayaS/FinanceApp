using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.MenuQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.MenuResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.MenuHandlers
{
    public class GetAllMenuBarQueryHandler :BaseHandler, IRequestHandler<GetAllMenuBarQuery, IList<GetAllMenuBarQueryResult>>
    {
        private readonly IMenuService menuService;

        public GetAllMenuBarQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IMenuService menuService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.menuService = menuService;
        }

        public async Task<IList<GetAllMenuBarQueryResult>> Handle(GetAllMenuBarQuery request, CancellationToken cancellationToken)
        {
            return await menuService.GetAllMenusAsync();
        }
    }
}
