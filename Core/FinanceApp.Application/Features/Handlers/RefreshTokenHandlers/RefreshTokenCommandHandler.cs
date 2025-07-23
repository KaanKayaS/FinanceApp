using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.RefreshTokenCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Results.RefreshTokenResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.Tokens;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RefreshTokenHandlers
{
    public class RefreshTokenCommandHandler : BaseHandler, IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
    {
        private readonly IAuthService authService;

        public RefreshTokenCommandHandler(UserManager<User> userManager,
            IMapper mapper, IUnitOfWork unitOfWork, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, IAuthService authService) : base(mapper, unitOfWork, httpContextAccessor,logger)
        {
            this.authService = authService;
        }
        public async Task<RefreshTokenCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
        }
    }
}
