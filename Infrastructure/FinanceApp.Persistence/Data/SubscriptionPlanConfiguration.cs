using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Data
{
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasData(
                          new SubscriptionPlan { Id = 1, DigitalPlatformId = 1, PlanType = SubscriptionType.Monthly , Price = 99.99m, },
                          new SubscriptionPlan { Id = 2, DigitalPlatformId = 1, PlanType = SubscriptionType.Yearly, Price = 999.99m },
                          new SubscriptionPlan { Id = 3, DigitalPlatformId = 2, PlanType = SubscriptionType.Monthly, Price = 79.99m },
                          new SubscriptionPlan { Id = 4, DigitalPlatformId = 2, PlanType = SubscriptionType.Yearly, Price = 899.99m }
                           );
        }
    }
}
 