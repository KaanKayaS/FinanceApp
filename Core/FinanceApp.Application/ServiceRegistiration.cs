using FinanceApp.Application.AutoMapper;
using FinanceApp.Application.Beheviors;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Features.Validator;
using FinanceApp.Application.Interfaces.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application
{
    public static class ServiceRegistiration
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistiration).Assembly));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr");

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

            services.AddTransient<ExceptionMiddleware>();
            services.AddTransient<AuthRules>();
            services.AddTransient<RefreshTokenRules>();
            services.AddTransient<CreditCardRules>();
            services.AddTransient<MembershipRules>();
            services.AddTransient<SubscriptionPlanRules>();
            services.AddTransient<ExpenseRules>();
            services.AddTransient<InstructionRules>();



        }
    }
}
