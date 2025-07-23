using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RegisterHandlers
{
    public class RegisterCommandHandler : BaseHandler, IRequestHandler<RegisterCommand, Unit>
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly AuthRules authRules;
        private readonly IAuthService authService;

        public RegisterCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,IAuthService authService, ILogger<AddBalanceCreditCardCommandHandler> logger) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authService = authService;
        }
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await authService.RegisterAsync(request);
            return Unit.Value;
        }
    }
}
