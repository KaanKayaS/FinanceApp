using AutoMapper;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests.BaseHandler
{
    public abstract class BaseHandlerTests
    {
        protected readonly Mock<IMapper> MapperMock = new();
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock = new();
        protected readonly Mock<IHttpContextAccessor> HttpContextAccessorMock = new();
        protected readonly Mock<AuthRules> AuthRulesMock = new();

        protected const string TestUserId = "42";
        protected readonly int TestUserIdInt = 42;

        protected BaseHandlerTests()
        {
            // HttpContext ve ClaimsPrincipal setup
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, TestUserId) }; // 42 idsini kullanıcı claimi oalrak ekliyoruz
            var identity = new ClaimsIdentity(claims, "TestAuthType");   // claimleri ClaimsIdentitye ekliyoruz
            var claimsPrincipal = new ClaimsPrincipal(identity);         // httpcontext.Userın  tutuğu bilgilere ekliyoruz
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };   // sahte httpcontext oluşturuyoruz 
            HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);  // mockumuzda sahte oluşturduğumuz httpcontexti dönüyoruz

            // AuthRules UserId setup
            AuthRulesMock.Setup(a => a.GetValidatedUserId(TestUserId)).ReturnsAsync(TestUserIdInt);
        }
    }

}
