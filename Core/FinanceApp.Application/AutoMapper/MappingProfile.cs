using AutoMapper;
using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Results.CreditCardResults;
using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.InstructionsResults;
using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Application.Features.Results.MenuResults;
using FinanceApp.Application.Features.Results.PaymentResults;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterCommand, User>()
              .ReverseMap();


            CreateMap<Memberships, GetAllMembershipsByUserQueryResult>()
             .ForMember(dest => dest.DigitalPlatformId, opt => opt.MapFrom(src => src.DigitalPlatformId))
             .ForMember(dest => dest.DigitalPlatformName, opt => opt.MapFrom(src => src.SubscriptionPlan.DigitalPlatform.Name))
             .ForMember(dest => dest.SubscriptionPlanName, opt => opt.MapFrom(src => src.SubscriptionPlan.PlanType))
             .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
             .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
             .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.SubscriptionPlan.DigitalPlatform.ImagePath))
             .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
             .ReverseMap();

            CreateMap<GetAllCreditCardsByUserQueryResult, CreditCard>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CardId))
             .ReverseMap();

            CreateMap<Payment, GetPaymentsByCardIdQueryResult>()
             .ForMember(dest => dest.DigitalPlatformName, opt => opt.MapFrom(src => src.Memberships.SubscriptionPlan.DigitalPlatform.Name))
             .ForMember(dest => dest.SubscriptionPlanName, opt => opt.MapFrom(src => src.Memberships.SubscriptionPlan.PlanType))
             .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
             .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
             .ReverseMap();


            CreateMap<BalanceMemory, GetPaymentsByCardIdQueryResult>()
            .ForMember(dest => dest.DigitalPlatformName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.AddBalanceCategory, opt => opt.MapFrom(src => src.AddBalanceCategory))
            .ReverseMap();


            CreateMap<DigitalPlatform, GetAllDigitalPlatfomQueryResult>()
            .ReverseMap();

            CreateMap<Instructions, InstructionDto>()
            .ReverseMap();

            CreateMap<GetAllExpenseAndPaymentByUserQueryResult, TheMostExpensiveExpenseDto>()
            .ReverseMap();

            CreateMap<Expens, GetAllExpenseAndPaymentByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Payment, GetAllExpenseAndPaymentByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Memberships.DigitalPlatform.Name))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Instructions, GetAllExpenseAndPaymentByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<BalanceMemory, GetAllExpenseAndPaymentByUserQueryResult>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
           .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
           .ReverseMap();

            CreateMap<Instructions, GetLast3ExpenseByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Payment, GetLast3ExpenseByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Memberships.DigitalPlatform.Name))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Expens, GetLast3ExpenseByUserQueryResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();


            CreateMap<Instructions, GetLastMonthExpenseTotalAmountQueryResult>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Payment, GetLastMonthExpenseTotalAmountQueryResult>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Expens, GetLastMonthExpenseTotalAmountQueryResult>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ReverseMap();

            CreateMap<Expens, GetAllExpenseByUserQueryResult>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ReverseMap();

            CreateMap<Instructions, GetAllInstructionsByUserQueryResult>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ReverseMap();

            CreateMap<Menu, GetAllMenuBarQueryResult>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
           .ReverseMap();

        }
    }
}
