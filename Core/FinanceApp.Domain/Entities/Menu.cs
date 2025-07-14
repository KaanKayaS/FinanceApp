using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }

        public virtual Menu Parent { get; set; }
        public virtual ICollection<Menu> Children { get; set; }
    }
}
