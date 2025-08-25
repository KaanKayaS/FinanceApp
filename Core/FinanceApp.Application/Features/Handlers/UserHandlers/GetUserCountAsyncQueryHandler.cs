using FinanceApp.Application.Features.Queries.UserQueries;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.UserHandlers
{
    public class GetUserCountAsyncQueryHandler : IRequestHandler<GetUserCountAsyncQuery, int>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AuthRules authRules;
        private readonly IUserService userService;
        public GetUserCountAsyncQueryHandler(IHttpContextAccessor httpContextAccessor, AuthRules authRules, IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authRules = authRules;
            this.userService = userService;
        }
        public async Task<int> Handle(GetUserCountAsyncQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await userService.GetUserCountAsync(userId);
        }
    }
}
