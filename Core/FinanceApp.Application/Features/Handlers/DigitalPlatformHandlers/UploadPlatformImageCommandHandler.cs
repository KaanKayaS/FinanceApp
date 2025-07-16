using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.DigitalPlatformCommands;
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
    public class UploadPlatformImageCommandHandler : BaseHandler, IRequestHandler<UploadPlatformImageCommand, Unit>
    {
        public UploadPlatformImageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
        }

        public async Task<Unit> Handle(UploadPlatformImageCommand request, CancellationToken cancellationToken)
        {
            var platform = await unitOfWork.GetReadRepository<DigitalPlatform>().GetAsync(x => x.Id == request.PlatformId);
            if (platform == null)
                throw new Exception("Platform bulunamadı");

            if (request.File != null && request.File.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/platforms", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream, cancellationToken);
                }

                platform.ImagePath = $"images/platforms/{fileName}";
                await unitOfWork.GetWriteRepository<DigitalPlatform>().UpdateAsync(platform);
                await unitOfWork.SaveAsync();
            }

            return Unit.Value;
        }
    }
}
