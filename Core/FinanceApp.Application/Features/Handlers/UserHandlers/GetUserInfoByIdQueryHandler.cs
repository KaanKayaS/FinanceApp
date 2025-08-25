using FinanceApp.Application.Features.Queries.UserQueries;
using FinanceApp.Application.Features.Results.UserResults;
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
    public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, GetUserInfoByIdQueryResult>
    {
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public GetUserInfoByIdQueryHandler(AuthRules authRules, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        public async Task<GetUserInfoByIdQueryResult> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return  await userService.GetUserInfoById(userId);
        }
    }
}
