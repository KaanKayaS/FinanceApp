using FinanceApp.Application.Features.Results.LoginResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.LoginCommands
{
    public class LoginCommand : IRequest<LoginCommandResult>
    {
        [DefaultValue("kaan@info")]
        public string Email { get; set; }

        [DefaultValue("123456")]
        public string Password { get; set; }
    }
}
