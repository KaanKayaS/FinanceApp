using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.DigitalPlatformCommands
{
    public class UploadPlatformImageCommand: IRequest<Unit>
    {
        public int PlatformId { get; set; }
        public IFormFile File { get; set; }
    }
}
