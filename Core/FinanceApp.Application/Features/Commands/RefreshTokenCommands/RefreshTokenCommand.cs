using FinanceApp.Application.Features.Results.RefreshTokenResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenCommandResult>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
