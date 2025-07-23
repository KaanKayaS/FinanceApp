using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IDigitalPlatformService
    {
        Task<IList<GetAllDigitalPlatfomQueryResult>> GetAllAsync();
        Task UploadPlatformImageAsync(int platformId, IFormFile file, CancellationToken cancellationToken);

    }
}
