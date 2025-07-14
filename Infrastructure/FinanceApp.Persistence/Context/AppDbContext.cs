using FinanceApp.Domain.Entities;
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
        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions options) : base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade kapalı

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.DigitalPlatform)
                .WithMany(d => d.Memberships)
                .HasForeignKey(m => m.DigitalPlatformId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade kapalı

            modelBuilder.Entity<Memberships>()
                .HasOne(m => m.SubscriptionPlan)
                .WithMany(sp => sp.Memberships)
                .HasForeignKey(m => m.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade kapalı


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

        }
    }
}
