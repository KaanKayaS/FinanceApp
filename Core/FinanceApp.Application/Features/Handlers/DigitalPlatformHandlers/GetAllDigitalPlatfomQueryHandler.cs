using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.DigitalPlatformQueries;
using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.DigitalPlatformHandlers
{
    public class GetAllDigitalPlatfomQueryHandler : BaseHandler, IRequestHandler<GetAllDigitalPlatfomQuery, IList<GetAllDigitalPlatfomQueryResult>>
    {
        public GetAllDigitalPlatfomQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
        }

        public async Task<IList<GetAllDigitalPlatfomQueryResult>> Handle(GetAllDigitalPlatfomQuery request, CancellationToken cancellationToken)
        {
            var values = await unitOfWork.GetReadRepository<DigitalPlatform>().GetAllAsync();

            return mapper.Map<IList<GetAllDigitalPlatfomQueryResult>>(values);
        }
    }
}
