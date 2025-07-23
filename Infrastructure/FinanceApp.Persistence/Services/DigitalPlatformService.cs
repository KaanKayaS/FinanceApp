using AutoMapper;
using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class DigitalPlatformService : IDigitalPlatformService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DigitalPlatformService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IList<GetAllDigitalPlatfomQueryResult>> GetAllAsync()
        {
            var platforms = await unitOfWork.GetReadRepository<DigitalPlatform>().GetAllAsync();
            return mapper.Map<IList<GetAllDigitalPlatfomQueryResult>>(platforms);
        }

        public async Task UploadPlatformImageAsync(int platformId, IFormFile file, CancellationToken cancellationToken)
        {
            var platform = await unitOfWork.GetReadRepository<DigitalPlatform>().GetAsync(x => x.Id == platformId);
            if (platform == null)
                throw new Exception("Platform bulunamadı");

            if (file != null && file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/platforms", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                platform.ImagePath = $"images/platforms/{fileName}";
                await unitOfWork.GetWriteRepository<DigitalPlatform>().UpdateAsync(platform);
                await unitOfWork.SaveAsync();
            }
        }
    }
}
