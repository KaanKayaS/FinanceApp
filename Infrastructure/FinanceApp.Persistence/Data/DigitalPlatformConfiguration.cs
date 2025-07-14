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
    public class DigitalPlatformConfiguration : IEntityTypeConfiguration<DigitalPlatform>
    {
        public void Configure(EntityTypeBuilder<DigitalPlatform> builder)
        {
            builder.HasData(
                new DigitalPlatform
                {
                    Id = 1,
                    Name = "Netflix",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 2,
                    Name = "Amazon Prime Video",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 3,
                    Name = "Youtube Premium",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 4,
                    Name = "TOD",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 5,
                    Name = "Disney Plus",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 6,
                    Name = "HBO",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 7,
                    Name = "Spotify",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 8,
                    Name = "LinkedIn",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                },
                new DigitalPlatform
                {
                    Id = 9,
                    Name = "EXXEN",
                    CreatedDate = DateTime.Now.AddHours(3),
                    IsDeleted = false,
                }
                );
        }
    }
}
