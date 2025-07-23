using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.DigitalPlatformCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.DigitalPlatformHandlers
{
    public class UploadPlatformImageCommandHandler : BaseHandler, IRequestHandler<UploadPlatformImageCommand, Unit>
    {
        private readonly IDigitalPlatformService digitalPlatformService;

        public UploadPlatformImageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IDigitalPlatformService digitalPlatformService) : base(mapper, unitOfWork, httpContextAccessor , logger)
        {
            this.digitalPlatformService = digitalPlatformService;
        }

        public async Task<Unit> Handle(UploadPlatformImageCommand request, CancellationToken cancellationToken)
        {
            await digitalPlatformService.UploadPlatformImageAsync(request.PlatformId, request.File, cancellationToken);
            return Unit.Value;
        }
    }
}
