using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.PasswordCommand
{
    public class ForgotPasswordAsyncCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}
