using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public AppDbContext()
        {
            
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Memberships> Memberships { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<DigitalPlatform> DigitalPlatforms { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Expens> Expenses { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Instructions> Instructions { get; set; }
        public DbSet<BalanceMemory> BalanceMemories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<InvestmentPlan> InvestmentPlans { get; set; }
        public DbSet<InvestmentPlanPayment> InvestmentPlanPayments { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.DigitalPlatform)
                .WithMany(d => d.Memberships)
                .HasForeignKey(m => m.DigitalPlatformId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.SubscriptionPlan)
                .WithMany(sp => sp.Memberships)
                .HasForeignKey(m => m.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yapma hata aldım


            // CreditCard — User
            modelBuilder.Entity<CreditCard>()
                .HasOne(c => c.User)
                .WithMany(u => u.CreditCards)
                .HasForeignKey(c => c.UserId);

            // Payment — Memberships
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Memberships)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MembershipsId);

            // Payment — CreditCard
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.CreditCard)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CreditCardId);

            // DigitalPlatform — SubscriptionPlan
            modelBuilder.Entity<SubscriptionPlan>()
                .HasOne(sp => sp.DigitalPlatform)
                .WithMany(dp => dp.SubscriptionPlans)
                .HasForeignKey(sp => sp.DigitalPlatformId);

            // Balance için decimal tip ayarı
            modelBuilder.Entity<CreditCard>()
                .Property(c => c.Balance)
                .HasColumnType("decimal(18,2)");

            // SubscriptionPlan Price için decimal tip ayarı
            modelBuilder.Entity<SubscriptionPlan>()
                .Property(sp => sp.Price)
                .HasColumnType("decimal(18,2)");

            // AuditLog — User ilişkisi (opsiyonel)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull); // User silinirse audit log'da UserId null olacak

            // AuditLog için metin alanları konfigürasyonu
            modelBuilder.Entity<AuditLog>()
                .Property(a => a.RequestData)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.ResponseData)
                .HasColumnType("nvarchar(max)");


            modelBuilder.Entity<InvestmentPlanPayment>()
                .HasOne(p => p.InvestmentPlan)
                .WithMany(p => p.InvestmentPlanPayments)
                .HasForeignKey(p => p.InvestmentPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvestmentPlanPayment>()
                .HasOne(p => p.CreditCard)
                .WithMany(c => c.InvestmentPlanPayments)
                .HasForeignKey(p => p.CreditCardId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}