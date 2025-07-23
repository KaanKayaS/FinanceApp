using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.DigitalPlatformQueries;
using FinanceApp.Application.Features.Results.DigitalPlatformResults;
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
    public class GetAllDigitalPlatfomQueryHandler : BaseHandler, IRequestHandler<GetAllDigitalPlatfomQuery, IList<GetAllDigitalPlatfomQueryResult>>
    {
        private readonly IDigitalPlatformService digitalPlatformService;

        public GetAllDigitalPlatfomQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            IDigitalPlatformService digitalPlatformService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.digitalPlatformService = digitalPlatformService;
        }

        public async Task<IList<GetAllDigitalPlatfomQueryResult>> Handle(GetAllDigitalPlatfomQuery request, CancellationToken cancellationToken)
        {
            return await digitalPlatformService.GetAllAsync();
        }
    }
}
