using FinanceApp.Application.Interfaces.Hangfire;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.AI;
using FinanceApp.Persistence.Context;
using FinanceApp.Persistence.Hangfire;
using FinanceApp.Persistence.Repositories;
using FinanceApp.Persistence.Services;
using FinanceApp.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinanceApp.Persistence
{
    public static class Registration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
              opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 2;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<Role>()
                .AddTokenProvider<EmailTokenProvider<User>>("Default")
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IInstructionService, InstructionService>();
            services.AddScoped<ICreditCardService, CreditCardService>();
            services.AddScoped<IDigitalPlatformService, DigitalPlatformService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IInvestmentPlanService, InvestmentPlanService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPdfReportService, PdfReportService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            services.AddScoped<AIService>(); // AI Service DI kaydı


            services.AddScoped<IMembershipRenewalService, MembershipRenewalService>(); // hangfire servisi
        }
    }
}
