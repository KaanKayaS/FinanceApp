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
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasData(
                        new Menu { Id = 1, Name="Kart işlemleri",ParentId= null, Order=1 },
                        new Menu { Id = 2, Name="Kartlarım",ParentId = 1, Order = 1},
                        new Menu { Id = 3, Name = "Bakiye Yükle", ParentId = 1, Order = 2 },
                        new Menu { Id = 4, Name = "Kart Ekle", ParentId = 1, Order = 3 },
                        new Menu { Id = 5, Name = "Gider İşlemleri", ParentId = null, Order =2 },
                        new Menu { Id = 6, Name = "Gider Ajandası", ParentId = 5, Order = 1 },
                        new Menu { Id = 7, Name = "Gider Ekle", ParentId = 5, Order = 2 },
                        new Menu { Id = 8, Name = "Ödeme Geçmişim", ParentId = 5, Order = 3 },
                        new Menu { Id = 9, Name = "Talimat İşlemleri", ParentId = null, Order = 3 },
                        new Menu { Id = 10, Name = "Talimat Oluştur", ParentId = 9, Order = 1 },
                        new Menu { Id = 11, Name = "Talimatlarım", ParentId = 9, Order = 2 },
                        new Menu { Id = 12, Name = "Abone İşlemleri", ParentId = null, Order = 4},
                        new Menu { Id = 13, Name = "Aboneliklerim", ParentId = 12, Order = 1 },
                        new Menu { Id = 14, Name = "Dijital Platform Aboneliği", ParentId = 12, Order = 2 }
                        );
        }
    }
}
