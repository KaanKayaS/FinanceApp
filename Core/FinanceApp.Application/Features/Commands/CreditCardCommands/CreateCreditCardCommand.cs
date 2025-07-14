using FinanceApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.CreditCardCommands
{
    public class CreateCreditCardCommand : IRequest<Unit>
    {
        public string CardNo { get; set; }
        public string ValidDate { get; set; }
        public string Cvv { get; set; }
        public string NameOnCard { get; set; }
    }
}
