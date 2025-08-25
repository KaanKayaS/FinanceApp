using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class test : ControllerBase
    {
        private readonly IMailService mailServices;

        public test(IMailService mailServices)
        {
            this.mailServices = mailServices;
        }
    }
}
