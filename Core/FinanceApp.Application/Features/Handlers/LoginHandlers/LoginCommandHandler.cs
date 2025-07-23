using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.LoginCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Results.LoginResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.Tokens;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.LoginHandlers
{
    public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(AuthRules authRules, UserManager<User> userManager,
            IMapper mapper, IUnitOfWork unitOfWork, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, IAuthService authService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authService = authService;
        }
        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await authService.LoginAsync(request.Email, request.Password);
        }
    }
}
